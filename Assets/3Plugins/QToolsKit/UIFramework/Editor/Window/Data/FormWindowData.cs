using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using _3Plugins.QToolsKit.UIFramework.Scripts;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public static class FormWindowData
    {
        public static GlobalUIWindowData WindowData { get; set; }
        public static PrefabTreeView CurrentTreeView { get; set; }
        public static TreeViewState CurrentTreeViewState { get; set; }


        public static WindowArea windowArea = WindowArea.Base;

        public static void Init()
        {
            WindowData = AssetDatabase.LoadAssetAtPath<GlobalUIWindowData>(
                "Assets/3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Configs/GlobalUIWindowData.asset");
            if (WindowData is null)
            {
                EditorUtils.CreateScriptableObject<GlobalUIWindowData>(
                    "Assets/3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Configs/GlobalUIWindowData.asset");
                WindowData = AssetDatabase.LoadAssetAtPath<GlobalUIWindowData>(
                    "Assets/3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Configs/GlobalUIWindowData.asset");
            }

            if (CurrentTreeViewState == null)
                CurrentTreeViewState = new TreeViewState();

            CheckWindowData();
        }

        /// <summary>
        /// 查询预制件数据
        /// </summary>
        /// <returns></returns>
        public static PrefabCacheData QueryPrefabData(GameObject prefab)
        {
            if (prefab is null) return null;

           
            if (!IsLeagalComp(prefab))
            {
                EditorUtility.DisplayDialog("警告", $"{prefab.name} 不是一个有效的UIForm预制件！",
                    "确定");
                return null;
            }
            if (!IsLeagalPrefabName(prefab))
            {
                EditorUtility.DisplayDialog("警告", $"{prefab.name} 预制件命名不符合规则，请以Form结尾! 示例:LoginForm",
                    "确定");
                return null;
            }
            CheckWindowData();
            CurrentTreeView = new PrefabTreeView(CurrentTreeViewState, prefab);
            foreach (var VARIABLE in WindowData.AllPrefabCacheDatas)
            {
                if (VARIABLE.name.Equals($"{prefab.name}_CacheData"))
                {
                    Debug.Log("找到了");
                    return VARIABLE;
                }
            }

            //这是一个新的预制件
            PrefabCacheData data = EditorUtils.CreateScriptableObject<PrefabCacheData>(
                $"Assets/3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Configs/Prefab/{prefab.name}_CacheData.asset");

            Debug.Log("创建文件====");
            //绘制树
            
            data.New(prefab, CurrentTreeView.AllItems, CurrentTreeView);
            EditorUtility.SetDirty(WindowData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            WindowData.AllPrefabCacheDatas.Add(data);
            return data;
        }

        /// <summary>
        /// 检查传入的prefab是否合法?
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        private static bool IsLeagalComp(GameObject prefab)
        {
            return prefab.GetComponent<QUIForm>();
        }
        /// <summary>
        /// 检查传入的prefab是否合法?
        /// </summary>
        /// <param name="prefab"></param>
        /// <returns></returns>
        private static bool IsLeagalPrefabName(GameObject prefab)
        {
            return prefab.name.Contains("Form");
        }
        /// <summary>
        /// 检查windowdata数据，将prefab引用为null的数据删除。
        /// </summary>
        private static void CheckWindowData()
        {
            for (int i = 0; i < WindowData.AllPrefabCacheDatas.Count; i++)
            {  if (WindowData.AllPrefabCacheDatas[i] is null)
                {
                    Debug.Log("删除miss");
                    WindowData.AllPrefabCacheDatas.Remove(WindowData.AllPrefabCacheDatas[i]);
                }
            }
        }
    }
}