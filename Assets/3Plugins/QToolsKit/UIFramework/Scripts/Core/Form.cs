using _3Plugins.QToolsKit.Loader;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public class Form : MonoBehaviour
    {
        [SerializeField]
        private string abPath;
        [SerializeField]
        private ViewEnum viewEnum;

        public GameObject GetPrefab()
        {
            return AssetBundleLoaderMgr.instance.LoadAsset<GameObject>(abPath, gameObject.name);
        }
    }
}