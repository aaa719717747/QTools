using System;
using System.Collections.Generic;
using System.IO;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using _3Plugins.QToolsKit.UIFramework.Scripts.Config;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using Unity.Plastic.Newtonsoft.Json;
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
        public static PrefabReflectionData ReflectionData { get; set; }
        public static PrefabCacheData CurrentPrefabCacheData { get; set; }
        
        public static int CurrentClikNodeId { get; set; }
        public static WindowArea windowArea = WindowArea.Base;
        private static string PrefabSavedPath = String.Empty;
        private static string ABSavedPath = String.Empty;

        public static void Init()
        {
            PrefabSavedPath = Path.Combine(Application.dataPath,
                "3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Json/UIEditorSavedData.bytes");
            ABSavedPath = Path.Combine(Application.dataPath,
                "3Plugins/QToolsKit/UIFramework/Editor/Window/Data/Json/UIEditorSavedData_AB.bytes");
            //json存储
            if (File.Exists(ABSavedPath))
            {
                string fileContent = File.ReadAllText(ABSavedPath);
                ReflectionData = JsonConvert.DeserializeObject<PrefabReflectionData>(fileContent);
            }
            else
            {
                PrefabReflectionData gwd = new PrefabReflectionData();
                string serializeContent = JsonConvert.SerializeObject(gwd);
                File.WriteAllText(ABSavedPath, serializeContent);
                ReflectionData = JsonConvert.DeserializeObject<PrefabReflectionData>(serializeContent);
            }
            //json存储
            if (File.Exists(PrefabSavedPath))
            {
                string fileContent = File.ReadAllText(PrefabSavedPath);
                WindowData = JsonConvert.DeserializeObject<GlobalUIWindowData>(fileContent);
            }
            else
            {
                GlobalUIWindowData gwd = new GlobalUIWindowData();
                string serializeContent = JsonConvert.SerializeObject(gwd);
                File.WriteAllText(PrefabSavedPath, serializeContent);
                WindowData = JsonConvert.DeserializeObject<GlobalUIWindowData>(serializeContent);
            }

            if (CurrentTreeViewState == null)
                CurrentTreeViewState = new TreeViewState();

            CheckWindowData();
        }

        public static void Saved()
        {
            File.WriteAllText(PrefabSavedPath, JsonConvert.SerializeObject(WindowData));
            File.WriteAllText(ABSavedPath, JsonConvert.SerializeObject(ReflectionData));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
           
        }

        /// <summary>
        /// 更新预制件数据
        /// </summary>
        public static void Update()
        {
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
            //查询预制件数据是否存在？
            foreach (PrefabData variable in WindowData.mPrefabsCacheDatas)
            {
                if (variable.mInstanceId == prefab.GetInstanceID())
                {
                    Debug.Log("找到了");
                    //更新数据
                    variable.mCacheData.Update(prefab, CurrentTreeView);
                    return variable.mCacheData;
                }
            }

            //这是一个新的预制件,创建新数据文件
            PrefabCacheData data = new PrefabCacheData();
            data.New(prefab, CurrentTreeView);
            WindowData.mPrefabsCacheDatas.Add(new PrefabData
            {
                mInstanceId = prefab.GetInstanceID(),
                mCacheData = data
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return data;
        }

        /// <summary>
        /// 根据Component类型实例化指定的SoEvents
        /// </summary>
        /// <param name="target"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static List<SOEvent> GetNewEventsByComponent(GameObject target, Component component)
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
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerUp,
                        mehtodName = $"{target.name}_OnPointerUp"
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.PointerEnter,
                        mehtodName = $"{target.name}_OnPointerEnter"
                    },
                    new SOEvent
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
                    },
                    new SOEvent
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
                    },
                    new SOEvent
                    {
                        isSetup = false,
                        eventEnum = SoEventEnum.ToggleOnValueChanged,
                        mehtodName = $"{target.name}_OnValueChanged"
                    },
                };
            }
            else if (component is Canvas)
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
            for (int i = 0; i < WindowData.mPrefabsCacheDatas.Count; i++)
            {
                if (WindowData.mPrefabsCacheDatas[i].mCacheData == null)
                {
                    Debug.Log("删除miss");
                    WindowData.mPrefabsCacheDatas.Remove(WindowData.mPrefabsCacheDatas[i]);
                }
            }
        }
    }
}