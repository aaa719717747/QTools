using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    [CreateAssetMenu(fileName = "PrefabCacheData",menuName = "PrefabCacheData(ScriptableObject)")]
    public class PrefabCacheData: ScriptableObject
    {
        public GameObject prefab;
        public List<SOTreeViewNodeData> treeViewNodes = new List<SOTreeViewNodeData>();
    }
    /// <summary>
    /// 树节点缓存数据
    /// </summary>
    [System.Serializable]
    public class SOTreeViewNodeData
    {
        //一个节点的控件设置与事件监听数据
        [FormerlySerializedAs("SoComponents")] public List<SOComponent> soComponents = new List<SOComponent>();
       
    }

    [System.Serializable]
    public class SOComponent
    {
        public bool isSetup;
        public int instanceId;
        public SOComponentType componentType;
        [FormerlySerializedAs("SoEvents")] public List<SOEvent> soEvents = new List<SOEvent>();
    }
    [System.Serializable]
    public struct SOEvent
    {
        public bool isSetup;
        public int instanceId;
        public SOEventType type;
    }

}