using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    [System.Serializable]
    public struct SOTreeViewItem
    {
        public bool isSetup;
        
    }

    public class PrefabCacheData
    {
        public GameObject prefab;
        /// <summary>
        /// ID,Data
        /// </summary>
        public Dictionary<int,SOTreeViewItem> allNodesDicts = new Dictionary<int,SOTreeViewItem>();
        
        
        // public void 
    }
}