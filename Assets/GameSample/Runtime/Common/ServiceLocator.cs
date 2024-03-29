using System;
using System.Collections.Generic;

namespace GameSample.Common
{
    /// <summary>
    /// サービスロケータ
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// コンテナ
        /// </summary>
        private static readonly Dictionary<Type, object> Container;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ServiceLocator()
        {
            Container = new Dictionary<Type, object>();
        }

        /// <summary>
        /// サービス取得
        /// </summary>
        public static T Resolve<T>()
        {
            return (T) Container[typeof(T)];
        }

        /// <summary>
        /// サービス登録
        /// </summary>
        public static void Register<T>(T instance)
        {
            Container[typeof(T)] = instance;
        }

        /// <summary>
        /// サービス登録解除
        /// </summary>
        public static void UnRegister<T>()
        {
            Container.Remove(typeof(T));
        }
    }
}