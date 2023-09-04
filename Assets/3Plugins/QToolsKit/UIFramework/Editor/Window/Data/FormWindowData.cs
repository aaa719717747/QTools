using System.Collections.Generic;
using System.Linq;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using _3Plugins.QToolsKit.UIFramework.Scripts;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public static class FormWindowData
    {
        public static GlobalUIWindowData WindowData { get; set; }
        public static PrefabTreeView CurrentTreeView { get; set; }
        public static TreeViewState CurrentTreeViewState { get; set; }

        public static PrefabCacheData CurrentPrefabCacheData{ get; set; }

        public static int CurrentClikNodeId{ get; set; }
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

        public static List<SOEvent> GetNewEventsByComponent(GameObject target,Component component)
        {
            if (component is RectTransform)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    }
                };
            }
            else if (component is Button)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.Button,
                        mehtodName = $"{target.name}_btn"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerClick,
                        mehtodName = $"{target.name}_OnPointerClick"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerDown,
                        mehtodName = $"{target.name}_OnPointerDown"
                    },new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerUp,
                        mehtodName = $"{target.name}_OnPointerUp"
                    }
                    ,new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerEnter,
                        mehtodName = $"{target.name}_OnPointerEnter"
                    }
                    ,new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerExit,
                        mehtodName = $"{target.name}_OnPointerExit"
                    }
                };
            }
            else if (component is Text)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.Text,
                        mehtodName = $"{target.name}_txt"
                    },
                };
            }
            else if (component is Image)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.Image,
                        mehtodName = $"{target.name}_img"
                    },
                };
            }
            else if (component is Slider)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.Slider,
                        mehtodName = $"{target.name}_img"
                    },new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.SliderOnValueChanged,
                        mehtodName = $"{target.name}_OnValueChanged"
                    },
                };
            }
            else if (component is Toggle)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.Toggle,
                        mehtodName = $"{target.name}_toggle"
                    },new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.ToggleOnValueChanged,
                        mehtodName = $"{target.name}_OnValueChanged"
                    },
                };
            }else if (component is QUIForm)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    }
                };
            }else if (component is Canvas)
            {
                return new List<SOEvent>
                {
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.RectTransform,
                        mehtodName = $"{target.name}_rf"
                    }
                };
            }
            return null;
        }

        /// <summary>
        /// 根据组件找到组件对应的数据
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        public static SOComponent CheckCompData(Component component,SOTreeViewNodeData data)
        {
            foreach (var variable in data.soComponents)
            {
                if (variable.instanceId==component.GetInstanceID())
                {
                    return variable;
                }
            }

            SOComponent newSoComp = new SOComponent
            {
                instanceId = component.GetInstanceID(),
                soEvents = GetNewEventsByComponent(CurrentTreeView.m_nowClikNodeObj, component)
            };
            //新的组件
            data.soComponents.Add(newSoComp);
            return newSoComp;
        }

        /// <summary>
        /// 检查节点变化
        /// </summary>
        /// <returns></returns>
        public static void CheckNodeChanged()
        {
            if (CurrentClikNodeId >= CurrentPrefabCacheData.treeViewNodes.Count)
            {
                int index = CurrentClikNodeId - (CurrentPrefabCacheData.treeViewNodes.Count - 1);
                //有新加的节点，需要更新
                for (int i = 0; i <index ; i++)
                {
                    SOTreeViewNodeData treeViewNode = new SOTreeViewNodeData();
                    treeViewNode.treeNodeId =CurrentPrefabCacheData.treeViewNodes.Count;
                    List<Component> comps = CurrentTreeView.ReturnSingleClickedItem(treeViewNode.treeNodeId).ToList();
                    GameObject clikGameObject = CurrentTreeView.ReturnSingleClickeGameObject(treeViewNode.treeNodeId);
                    CurrentTreeView.m_nowClikNodeObj = clikGameObject;
                    foreach (var VARIABLE2 in comps)
                    {
                        var soComp = FormWindowData.CheckCompData(VARIABLE2,treeViewNode);
                        treeViewNode.soComponents.Add(soComp);
                        soComp.soEvents = FormWindowData.GetNewEventsByComponent(clikGameObject, VARIABLE2);
                    }
                    CurrentPrefabCacheData.treeViewNodes.Add(treeViewNode);
                }
            }
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