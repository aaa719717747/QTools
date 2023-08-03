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
        private Dictionary<ViewEnum, FormUI> FormScripts = new Dictionary<ViewEnum, FormUI>();

        private void Start()
        {
            //初始化
            AssetBundleLoaderMgr.instance.Init();
            // InitViewPanels();
            TestCache();
            TestInstance();
            TestInstance3();
        }


        public void TestCache()
        {
            Profiler.BeginSample("##################11111111####################");
            for (int i = 0; i < 100; i++)
            {
                GameObject prefab =
                    AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/maincitypanel", "MainCityView");
                //实例化预设
                GameObject go = Instantiate(prefab, transform);
                Destroy(go);
            }

            Profiler.EndSample();
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
        public void TestInstance3()
        {
            Profiler.BeginSample("##################33333333####################");
            GameObject prefab =
                AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/maincitypanel", "MainCityView");
            for (int i = 0; i < 100; i++)
            {
                //实例化预设
                GameObject go = Instantiate(prefab, transform);
                // Destroy(go);
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
            FormUI uiForm = go.GetComponent<FormUI>();
            uiForm.Bind(
                new LoginView(),
                new LoginModel());
            uiForm.viewEnum = ViewEnum.LOGIN;
            FormScripts.Add(ViewEnum.LOGIN, uiForm);
        }

        public void ShowPanel(ViewEnum viewEnum)
        {
            if (FormScripts.ContainsKey(viewEnum))
            {
                FormScripts[viewEnum].OnShow();
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