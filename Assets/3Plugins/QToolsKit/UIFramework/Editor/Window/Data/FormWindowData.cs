using System.Collections.Generic;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public static class FormWindowData
    {
        //临时变量
        public static GameObject m_targetPrefab;
        public static Component m_nowClikComponent;
        public static List<Component> m_currentNodeComponents = new List<Component>();
        
        public static void m_UpdateNodeComponentsList(List<Component> components)
        {
            m_currentNodeComponents.Clear();
            m_currentNodeComponents = components;
        }
        
        
        
        
        //持久化数据
        public static PrefabCacheData PrefabData { get; set; }
        public static GlobalUIWindowData WindowData { get; set; }

    }
}