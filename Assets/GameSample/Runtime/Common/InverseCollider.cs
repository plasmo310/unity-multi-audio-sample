using System.Linq;
using UnityEngine;

namespace GameSample.Common
{
    /// <summary>
    /// 反転コライダ
    /// </summary>
    public class InverseCollider : MonoBehaviour
    {
        private void Awake()
        {
            CreateInvertedMeshCollider();
        }

        private void CreateInvertedMeshCollider()
        {
            RemoveExistingColliders();
            InvertMesh();

            gameObject.AddComponent<MeshCollider>();
        }

        private void RemoveExistingColliders()
        {
            var colliders = GetComponents<Collider>();
            foreach (var col in colliders)
            {
                DestroyImmediate(col);
            }
        }

        private void InvertMesh()
        {
            var mesh = GetComponent<MeshFilter>().mesh;
            mesh.triangles = mesh.triangles.Reverse().ToArray();
        }
    }
}
