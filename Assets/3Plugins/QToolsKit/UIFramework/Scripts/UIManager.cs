using System;
using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Sample;
using QToolsKit.Loader;
using UnityEngine;
using UnityEngine.Profiling;

namespace _3Plugins.QToolsKit.UIFramework.Scripts
{
    public class UIManager : MonoBehaviour
    {
        private Dictionary<ViewEnum, IForm> _forms = new Dictionary<ViewEnum, IForm>();

        private void Start()
        {
            //初始化
            AssetBundleLoaderMgr.instance.Init();
            TestInstance();
        }

        public void LoadUIPrefabs()
        {
        }


        public void TestInstance()
        {
            Profiler.BeginSample("##################22222222####################");
            GameObject prefab =
                AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/maincitypanel", "MainCityView");
            for (int i = 0; i < 100; i++)
            {
                //实例化预设
                GameObject go = Instantiate(prefab, transform);
                go.hideFlags = HideFlags.HideInHierarchy;
                Destroy(go);
            }

            Profiler.EndSample();
        }


        public void InitViewPanels()
        {
            //加载预设
            GameObject prefab =
                AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/maincitypanel", "MainCityView");
            //实例化预设
            GameObject go = Instantiate(prefab, transform);
            LoginForm uiForm = go.GetComponent<LoginForm>();
            uiForm.Bind(
                new LoginView(),
                new LoginModel());
            uiForm.viewEnum = ViewEnum.LOGIN;
            _forms.Add(ViewEnum.LOGIN, uiForm);
        }

        public void ShowPanel(ViewEnum viewEnum)
        {
            if (_forms.ContainsKey(viewEnum))
            {
                _forms[viewEnum].OnShow();
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("show"))
            {
                ShowPanel(ViewEnum.LOGIN);
            }
        }
    }
}