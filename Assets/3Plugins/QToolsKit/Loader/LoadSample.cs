using System;
using _3Plugins.QToolsKit.Loader;
using UnityEngine;

namespace QToolsKit.Loader
{
    public class LoadSample : MonoBehaviour
    {
        [Header("标签名：")]
        public string assetBundle;
        [Header("预制体名：")]
        public string prefabName;
        void Awake()
        {
            //初始化
            AssetBundleLoaderMgr.Instance.Init();
        }
 
        void LoadAsset()
        {
            //加载预设
            GameObject prefab = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(assetBundle, prefabName);
            //实例化预设
            Instantiate(prefab);
        }
        void OnGUI()
        {
            if (GUI.Button(new Rect(100, 100, 100, 50), "LoadAsset"))
            {
                LoadAsset();
            }
        }
    }
}