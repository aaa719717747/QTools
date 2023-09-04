using System.Collections.Generic;
using System.Linq;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json
{
    [CreateAssetMenu(fileName = "PrefabCacheData", menuName = "PrefabCacheData(ScriptableObject)")]
    public class PrefabCacheData : ScriptableObject
    {
        public GameObject prefab;
        public List<SOTreeViewNodeData> treeViewNodes = new List<SOTreeViewNodeData>();

        public void New(GameObject prefab, List<TreeViewItem> AllItems, PrefabTreeView CurrentTreeView)
        {
            this.prefab = prefab;
            foreach (var VARIABLE in AllItems)
            {
                SOTreeViewNodeData treeViewNode = new SOTreeViewNodeData();
                treeViewNode.treeNodeId = VARIABLE.id;
                List<Component> comps = CurrentTreeView.ReturnSingleClickedItem(VARIABLE.id).ToList();
                GameObject clikGameObject = CurrentTreeView.ReturnSingleClickeGameObject(VARIABLE.id);
                CurrentTreeView.m_nowClikNodeObj = clikGameObject;
                foreach (var VARIABLE2 in comps)
                {
                    var soComp = FormWindowData.CheckCompData(VARIABLE2,treeViewNode);
                    treeViewNode.soComponents.Add(soComp);
                    soComp.soEvents = FormWindowData.GetNewEventsByComponent(clikGameObject, VARIABLE2);
                }

                treeViewNodes.Add(treeViewNode);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }


    /// <summary>
    /// 树节点缓存数据
    /// </summary>
    [System.Serializable]
    public class SOTreeViewNodeData
    {
        [FormerlySerializedAs("NodeId")] public int treeNodeId;

        //一个节点的控件设置与事件监听数据
        [FormerlySerializedAs("SoComponents")] public List<SOComponent> soComponents = new List<SOComponent>();
    }

    [System.Serializable]
    public class SOComponent
    {
        [FormerlySerializedAs("InstanceId")] public int instanceId;
        [FormerlySerializedAs("SoEvents")] public List<SOEvent> soEvents = new List<SOEvent>();
    }

    [System.Serializable]
    public struct SOEvent
    {
        public bool isSetup;
        public SoEventEnum eventEnum;
        public string mehtodName;
    }
}