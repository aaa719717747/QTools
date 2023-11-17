using System;
using System.Collections.Generic;
using _3Plugins.QToolsKit.Loader;
using _3Plugins.QToolsKit.UIFramework.Scripts.Config;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public class UIMgr : MonoBehaviour
    {
        public PrefabReflectionData mPrefabReflectionData;

        private void Awake()
        {
            AssetBundleLoaderMgr.Instance.Init();
        }

        private void Start()
        {
            OpenForm(ViewEnum.LoginForm);
        }

        public void OpenForm(ViewEnum mViewEnum, UserTB userData = null)
        {
            FormData abData = mPrefabReflectionData.GetFormData(mViewEnum);
            GameObject prefab = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(abData);
            GameObject go = Instantiate(prefab, this.transform);
            go.transform.SetAsLastSibling();
            go.GetComponent<QUIForm>().OnOpen(userData);
        }
    }
}