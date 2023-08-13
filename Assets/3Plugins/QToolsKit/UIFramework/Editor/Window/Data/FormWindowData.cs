using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using UnityEditor;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public static class FormWindowData
    {
        public static PrefabCacheData PrefabData
        {
            get=>AssetDatabase.LoadAssetAtPath<PrefabCacheData>("");
            set
            {
                
            }
        }
        public static GlobalUIWindowData WindowData { get; set; }
        public static WindowArea windowArea = WindowArea.Base;
        public static GameObject m_targetPrefab;
        public static Component m_nowClikComponent;
        public static List<Component> m_currentNodeComponents = new List<Component>();


        public static void m_UpdateNodeComponentsList(List<Component> components)
        {
            m_currentNodeComponents.Clear();
            m_currentNodeComponents = components;
        }
    }
}