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


        public void New(GameObject prefab, PrefabTreeView CurrentTreeView)
        {

            this.prefab = prefab;
            Debug.Log(CurrentTreeView.AllItems.Count);
            foreach (TreeViewItem VARIABLE in CurrentTreeView.AllItems)
            {
                SOTreeViewNodeData treeViewNode = new SOTreeViewNodeData();
                GameObject clikGameObject = CurrentTreeView.ReturnSingleClickeGameObject(VARIABLE.id);
                treeViewNode.treeNodeInstanceId = clikGameObject.GetInstanceID();
                Component[] comps = CurrentTreeView.ReturnSingleClickedItem(VARIABLE.id);
                List<Component> fifterComps = CurrentTreeView.FiterComponent(comps);
                foreach (var VARIABLE2 in fifterComps)
                {
                    SOComponent soComp = new SOComponent();
                    soComp.instanceId = VARIABLE2.GetInstanceID();
                    soComp.soEvents = FormWindowData.GetNewEventsByComponent(clikGameObject, VARIABLE2);
                    treeViewNode.soComponents.Add(soComp);
                }
                
                treeViewNodes.Add(treeViewNode);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void Update(GameObject mPrefab, PrefabTreeView mcurrentTreeView)
        {
            List<GameObject> allPrefabsChilds = new List<GameObject>();
            foreach (TreeViewItem VARIABLE in mcurrentTreeView.AllItems)
            {
                GameObject clikGameObject = mcurrentTreeView.ReturnSingleClickeGameObject(VARIABLE.id);
                allPrefabsChilds.Add(clikGameObject);
            }

            //双向对比策略(节点)
            //数据和实时对比
            //单向:数据->实时
            for (int i = 0; i < treeViewNodes.Count; i++)
            {
                GameObject targetObj = null;
                bool isExit = IsExitTargetInstanceIdWithGameObject(treeViewNodes[i].treeNodeInstanceId,
                    allPrefabsChilds, out targetObj);
                if (!isExit)
                {
                    //不存在（则节点被删除）数据中删除此节点
                    treeViewNodes.Remove(treeViewNodes[i]);
                }
                else
                {
                    //双向对比策略(组件)
                    for (int j = 0; j < treeViewNodes[i].soComponents.Count; j++)
                    {
                        List<Component> copms = mcurrentTreeView.FiterComponent(targetObj.GetComponents<Component>());
                        if (!IsExitTargetInstanceIdWithComponent(treeViewNodes[i].soComponents[j].instanceId, copms))
                        {
                            //不存在（则组件被删除）数据中删除此节点
                            treeViewNodes[i].soComponents.Remove(treeViewNodes[i].soComponents[j]);
                        }
                    }
                }
            }

            //逆向:实时->数据
            for (int i = 0; i < allPrefabsChilds.Count; i++)
            {
                List<SOComponent> targetDataSoComponents = null;
                bool isExit =
                    IsExitTargetInstanceIdWithGameObject_Data(allPrefabsChilds[i].GetInstanceID(), this.treeViewNodes,out targetDataSoComponents);
                if (!isExit)
                {
                    //不存在（新增节点）数据中增加此节点
                    SOTreeViewNodeData treeViewNode = new SOTreeViewNodeData();

                    treeViewNode.treeNodeInstanceId = allPrefabsChilds[i].GetInstanceID();
                    Component[] comps = allPrefabsChilds[i].GetComponents<Component>();
                    List<Component> fifterComps = mcurrentTreeView.FiterComponent(comps);
                    foreach (var VARIABLE2 in fifterComps)
                    {
                        SOComponent soComp = new SOComponent();
                        soComp.instanceId = VARIABLE2.GetInstanceID();
                        soComp.soEvents = FormWindowData.GetNewEventsByComponent(allPrefabsChilds[i], VARIABLE2);
                        treeViewNode.soComponents.Add(soComp);
                    }

                    treeViewNodes.Add(treeViewNode);
                }
                else
                {
                    Component[] comps = allPrefabsChilds[i].GetComponents<Component>();
                    List<Component> fifterComps = mcurrentTreeView.FiterComponent(comps);
                    //双向对比策略(组件)
                    for (int j = 0; j < fifterComps.Count; j++)
                    {
                        if (!IsExitTargetInstanceIdWithComponent_Data(fifterComps[j].GetInstanceID(),
                                targetDataSoComponents))
                        {
                            //不存在（属性新增组件）数据中增加此组件
                            SOComponent soComp = new SOComponent();
                            soComp.instanceId = fifterComps[j].GetInstanceID();
                            soComp.soEvents =
                                FormWindowData.GetNewEventsByComponent(allPrefabsChilds[i], fifterComps[j]);
                            treeViewNodes[i].soComponents.Add(soComp);
                        }
                    }
                }
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private bool IsExitTargetInstanceIdWithGameObject_Data(int targetInstanceId,
            List<SOTreeViewNodeData> poolGameObjects,out List<SOComponent> targetDataSoComponents)
        {
            foreach (var VARIABLE in poolGameObjects)
            {
                if (targetInstanceId == VARIABLE.treeNodeInstanceId)
                {
                    targetDataSoComponents = VARIABLE.soComponents;
                    return true;
                }
            }

            targetDataSoComponents = null;
            return false;
        }

        private bool IsExitTargetInstanceIdWithGameObject(int targetInstanceId, List<GameObject> poolGameObjects,
            out GameObject targetGameObject)
        {
            foreach (var VARIABLE in poolGameObjects)
            {
                if (targetInstanceId == VARIABLE.GetInstanceID())
                {
                    targetGameObject = VARIABLE;
                    return true;
                }
            }

            targetGameObject = null;
            return false;
        }

        private bool IsExitTargetInstanceIdWithComponent(int targetInstanceId, List<Component> poolComponents)
        {
            foreach (var VARIABLE in poolComponents)
            {
                if (targetInstanceId == VARIABLE.GetInstanceID())
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsExitTargetInstanceIdWithComponent_Data(int targetInstanceId, List<SOComponent> poolComponents)
        {
            foreach (var VARIABLE in poolComponents)
            {
                if (targetInstanceId == VARIABLE.instanceId)
                {
                    return true;
                }
            }

            return false;
        }

        public SOComponent GetCurrentClikSoComponentData(GameObject m_nowClikNodeObj, Component m_nowClikComponent)
        {
            foreach (var soTree in treeViewNodes)
            {
                if (soTree.treeNodeInstanceId == m_nowClikNodeObj.GetInstanceID())
                {
                    foreach (var VARIABLE in soTree.soComponents)
                    {
                        if (VARIABLE.instanceId == m_nowClikComponent.GetInstanceID())
                        {
                            return VARIABLE;
                        }
                    }
                }
            }

            Debug.LogError($"找不到对应的SOComponent，内部异常错误！");
            return null;
        }
    }


    /// <summary>
    /// 树节点缓存数据
    /// </summary>
    [System.Serializable]
    public class SOTreeViewNodeData
    {
        [FormerlySerializedAs("NodeId")] public int treeNodeInstanceId;

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
    public class SOEvent
    {
        public bool isSetup;
        public SoEventEnum eventEnum;
        public string mehtodName;
    }
}