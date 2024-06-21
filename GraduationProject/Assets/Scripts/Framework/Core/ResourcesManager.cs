using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace GameFrameWork
{
    public class ResourcesManager : SingletonMono<ResourcesManager>
    {
        public Object LoadAssetSync(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            Object obj = Resources.Load(assetPath);

#if UNITY_EDITOR
            if (obj is null)
            {
                Debug.LogWarningFormat("Resources文件夹下路径为：{0}的资源不存在。", assetPath);
            }
#endif
            return obj;
        }

        public T LoadAssetSync<T>(string assetPath) where T : Object
        {
            if (string.IsNullOrEmpty(assetPath)) return null;
            T obj = Resources.Load<T>(assetPath);

#if UNITY_EDITOR
            if (obj is null)
            {
                Debug.LogWarningFormat("Resources文件夹下路径为：{0}的{1}类型资源不存在。", assetPath, typeof(T));
            }
#endif
            return obj;
        }

        public void LoadAssetAsync(string assetPath, UnityAction<object> callback)
        {
            StartCoroutine(ReallyLoadAssetAsync(assetPath, callback));
        }

        public IEnumerator ReallyLoadAssetAsync(string assetPath, UnityAction<object> callback)
        {
            if (string.IsNullOrEmpty(assetPath)) yield break;
            ResourceRequest request = Resources.LoadAsync(assetPath);
            yield return request;
#if UNITY_EDITOR
            if (request is null || request.asset is null)
            {
                Debug.LogWarningFormat("Resources文件夹下路径为：{0}的资源不存在。", assetPath);
            }
#endif
            callback(request.asset);
        }

        public void LoadAssetAsync<T>(string assetPath, UnityAction<T> callback) where T : Object
        {
            StartCoroutine(ReallyLoadAssetAsync<T>(assetPath, callback));
        }

        public IEnumerator ReallyLoadAssetAsync<T>(string assetPath, UnityAction<T> callback) where T : Object
        {
            if (string.IsNullOrEmpty(assetPath)) yield break;
            ResourceRequest request = Resources.LoadAsync<T>(assetPath);
            yield return request;
#if UNITY_EDITOR
            if (request is null || request.asset is null)
            {
                Debug.LogWarningFormat("Resources文件夹下路径为：{0}的{1}类型资源不存在。", assetPath, typeof(T));
            }
#endif
            callback(request.asset as T);
        }
    }
}