/****************************************************************************
 *
 * Copyright (c) CRI Middleware Co., Ltd.
 *
 ****************************************************************************/

#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

[assembly: InternalsVisibleTo("CriMw.CriWare.Adxlipsync.Editor")]
[assembly: InternalsVisibleTo("Assembly-CSharp-Editor")]
[assembly: InternalsVisibleTo("AdxlipsyncRuntimeTests")]

namespace CriWare {

#if UNITY_2018_1_OR_NEWER && CRIWARE_TIMELINE_1_OR_NEWER
	namespace CriTimeline.Atom {

		/** <summary>Atom Timelineのプレビューを拡張するための基底クラス</summary> */
		internal abstract class CriAtomTimelinePreviewerExtensionBase {

			/** <summary>Update時のコンフィグ</summary> */
			public struct Config {
				/** <summary>プレビュー対象のAtomTrackに対応して作成されたMixerBehaviourを表すGuid</summary> */
				public Guid trackId;

				/** <summary>プレビュー再生時にトラックごとに生成されるAtomExPlayer</summary> */
				public CriAtomExPlayer atomExPlayer;

				/** <summary><see cref='GetExtensionTarget'/> のシーン上での実体</summary> */
				public Component targetComponent;
			}

			/** <summary>AtomTimelineを何に対して拡張するのか、その型を返す</summary>
			 * <returns>拡張先のクラス型</returns>
			 */
			public virtual Type GetExtensionTarget() { return null; }

			/** <summary>AtomTimelineの拡張クラスを更新する</summary> */
			public virtual void UpdateExtension(Config config) { }
		}

		/** <summary><see cref='CriAtomTimelinePreviewerExtensionBase'/> をAtomMixerBehaviourから呼び出すためのインターフェース<br/></summary>
		 */
		internal class CriAtomTimelinePreviewerExtensionManager {

			private static CriAtomTimelinePreviewerExtensionManager instance = null;
			public static CriAtomTimelinePreviewerExtensionManager Instance {
				get {
					if (instance == null) {
						instance = new CriAtomTimelinePreviewerExtensionManager();
						instance.SetUp();
					}
					return instance;
				}
			}

			/** <summary>継承先の情報<br/>
			 * <br/>
			 * Key<br/>
			 *   継承したクラスが操作したい型<br/>
			 * Value<br/>
			 *   CriAtomTimelinePreviewerExtensionBaseを継承したオブジェクトをアップキャストしたもの<br/></summary>
			 */
			private Dictionary<Type, CriAtomTimelinePreviewerExtensionBase> SubClassTable = null;

			/** <summary>サブクラスを見つけて、インスタンスにキャッシュする</summary> */
			private void SetUp() {
				if (SubClassTable == null) {
					SubClassTable = new Dictionary<Type, CriAtomTimelinePreviewerExtensionBase>();
				}
				var subClassList = GetSubClassList();
				bool isAllivedSubClass = !(subClassList == null || subClassList.Count == 0);
				if (!isAllivedSubClass) {
					return;
				}

				foreach (var subClass in subClassList) {
					var instance = Activator.CreateInstance(subClass, null) as CriAtomTimelinePreviewerExtensionBase;
					if (instance != null) {
						SubClassTable.Add(instance.GetExtensionTarget(), instance);
					}
				}
			}

			/** シングルトンの破棄 */
			public void InstanceDispose() {
				if (SubClassTable != null) {
					SubClassTable.Clear();
					foreach (var subClassInstance in SubClassTable.Values) {
						if (subClassInstance is IDisposable) {
							(subClassInstance as IDisposable).Dispose();
						}
					}
				}
				SubClassTable = null;

				instance = null;
			}

			~CriAtomTimelinePreviewerExtensionManager() {
				InstanceDispose();
			}

			/** <summary>Atom Timelineを拡張するクラスを更新する<br/>
			 * ProcessFrame()が呼び出し時の、対象Clipの範囲内にいる場合のみ呼び出される<br/></summary>
			 * <param name='bindObject'/>
			 * <param name='trackGuid'/>
			 * <param name='previewPlayer'/>
			 */
			public void UpdateExtensions(CriAtomSourceBase bindObject, Guid trackGuid, CriAtomExPlayer previewPlayer) {
				var config = new CriAtomTimelinePreviewerExtensionBase.Config();
				config.trackId = trackGuid;
				config.atomExPlayer = previewPlayer;
				foreach (var targetType in this.SubClassTable.Keys) {
					var refList = FindReference(bindObject, targetType);
					foreach (var referencedObject in refList) {
						if (this.SubClassTable.TryGetValue(referencedObject.type, out CriAtomTimelinePreviewerExtensionBase subClass)) {
							config.targetComponent = referencedObject.gameObject.GetComponent(referencedObject.type);
							subClass.UpdateExtension(config);
						}
					}
				}
			}

			private static List<Type> GetSubClassList() {
				var findResults = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(assem => assem.GetTypes())
					.Where(x => x.IsSubclassOf(typeof(CriAtomTimelinePreviewerExtensionBase)) && !x.IsAbstract)
					.ToList();

				return findResults;
			}

			private List<ReferenceFinder.ReferencedObjects> FindReference(CriAtomSourceBase criAtomSource, Type searchType) {
				return ReferenceFinder.FindMatchAllObjects(criAtomSource.gameObject, searchType);
			}

		}
	}
#endif

	internal class ReferenceFinder {
		public class ReferencedObjects {
			public GameObject gameObject;
			public Type type;
		}

		private static List<ReferencedObjects> foundReferences = null;

		public static List<ReferencedObjects> FindMatchAllObjects(UnityEngine.Object suspect, Type searchType) {
			foundReferences = new List<ReferencedObjects>();

			var foundComponents = GameObject.FindObjectsOfType(searchType)
				.Select(obj => (Component)obj)
				.ToList();

			int suspectInstanceId = suspect.GetInstanceID();
			foreach (var component in foundComponents) {
				var serializeObject = new SerializedObject(component);
				if (serializeObject == null) {
					continue;
				}
				var iterator = serializeObject.GetIterator();
				iterator.NextVisible(true); // first reference is Script
				FindSerializePropertyRecursive(iterator,suspectInstanceId, component);
			}

			return foundReferences;
		}

		private static void FindSerializePropertyRecursive(SerializedProperty iterator, int suspect, Component component) {
			while (iterator.NextVisible(true)) {
				if (iterator.isArray && iterator.type != "string") {
					FindSerializePropertyRecursive(iterator, suspect, component);
				}
				if (CompareGuid(iterator, suspect)) {
					ReferenceFinder.foundReferences.Add(new ReferencedObjects {
						gameObject = component.gameObject,
						type = component.GetType()
					});
				}
			}
		}

		private static bool CompareGuid(SerializedProperty property, int suspect) {
			if (property.propertyType != SerializedPropertyType.ObjectReference) {
				return false;
			}

			bool checkSubClass = property.objectReferenceValue is GameObject || property.objectReferenceValue is ScriptableObject;
			if (checkSubClass && property.objectReferenceValue.GetInstanceID() == suspect) {
				return true;
			} else if (property.objectReferenceValue is Component) {
				return (property.objectReferenceValue as Component)?.gameObject.GetInstanceID() == suspect;
			}
			return property.objectReferenceValue?.GetInstanceID() == suspect;
		}
	}

} // namespace CriWare

#endif
