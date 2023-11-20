using System;
using System.Collections.Generic;
using _3Plugins.QToolsKit.Loader;
using _3Plugins.QToolsKit.UIFramework.Scripts.Config;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public class UIMgr : MonoBehaviour
    {
        public PrefabReflectionData ReflectionUIData;
        private Dictionary<ViewEnum, QUIForm> Forms = new Dictionary<ViewEnum, QUIForm>();

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
            FormData abData = ReflectionUIData.GetFormData(mViewEnum);
            GameObject prefab = AssetBundleLoaderMgr.Instance.LoadAsset<GameObject>(abData);
            GameObject go = Instantiate(prefab, this.transform);
            go.transform.SetAsLastSibling();
            QUIForm form = go.GetComponent<QUIForm>();
            form.OnInit();
            form.OnOpen(userData);
            Forms.Add(mViewEnum, form);
        }

        public void CloseForm(ViewEnum mViewEnum)
        {
            Forms[mViewEnum].Close();
            Forms.Remove(mViewEnum);
        }
    }
}