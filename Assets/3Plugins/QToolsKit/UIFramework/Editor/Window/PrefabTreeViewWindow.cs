using System;
using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Enums;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using _3Plugins.QToolsKit.UIFramework.Scripts.Config;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window
{
    public class PrefabTreeViewWindow : EditorWindow
    {
        private Vector2 treeviewScrollPosition;
        private Vector2 rightScrollPosition;
        private Vector2 leftScrollPosition;
        private Vector2 centerScrollPosition;
        private Vector2 centerEventScrollPosition;
        private Vector2 centerReflectionScrollPosition;
        private bool isFoldout = true;
        private GameObject m_targetPrefab;

        private PrefabCacheData m_cacheData;
        /*
             * 快捷键的写法：
                # 代表 shift  
                & 代表  alt
                % 代表 Ctrl
                之后加上 大写的字母 
             */

        [MenuItem("项目工具/UI编辑器 %&Q")]
        static void ShowWindow()
        {
            if (IsInPrefabMode())
            {
                EditorUtility.DisplayDialog("警告", $"预制体编辑模式中不支持使用UI编辑器！请退出预制件编辑模式！",
                    "确定");
                return;
            }

            FormWindowData.Init();
            var window = GetWindow<PrefabTreeViewWindow>();
            window.titleContent = new GUIContent("UI编辑器");
            nowInstanceId = 0;
            window.Show();
        }

        private void OnDisable()
        {
            FormWindowData.Saved();
        }

        public static bool IsInPrefabMode()
        {
            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            return prefabStage != null;
        }

        private void OnGUI()
        {
            if (IsInPrefabMode())
            {
                EditorUtility.DisplayDialog("警告", $"预制体编辑模式中不支持使用UI编辑器！请退出预制件编辑模式！",
                    "确定");
                Close();
                return;
            }

            //顶部按钮
            DrawTopMenuBtnsGUI();


            switch (FormWindowData.windowArea)
            {
                case WindowArea.Base:
                    //==========================基础配置=========================
                    DrawDirectory();

                    DrawViewReflectionPrefab();
                    break;
                case WindowArea.Prefab:
                    //==========================预制件配置=========================
                    EditorGUILayout.BeginHorizontal();
                    // 左侧窗口
                    DrawLeftTreeViewWindowGUI();
                    // 右侧窗口
                    DrawDrawRightWindow_ComponentsGUI();
                    EditorGUILayout.EndHorizontal();
                    break;
            }
        }

        private static int nowInstanceId;

        /// <summary>
        /// 左侧目录树
        /// </summary>
        private void DrawLeftTreeViewWindowGUI()
        {
            // 左侧窗口
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.Width(400));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("导出为C#代码", GUILayout.Height(30)))
            {
                Debug.Log("Button 1 clicked");
            }

            if (GUILayout.Button("导出为Lua代码", GUILayout.Height(30)))
            {
                Debug.Log("Button 1 clicked");
            }

            EditorGUILayout.EndHorizontal();

            m_targetPrefab =
                (GameObject)EditorGUILayout.ObjectField(m_targetPrefab, typeof(GameObject),
                    GUILayout.ExpandWidth(true));

            if (m_targetPrefab != null && nowInstanceId != m_targetPrefab.GetInstanceID())
            {
                nowInstanceId = m_targetPrefab.GetInstanceID();
                m_cacheData = FormWindowData.QueryPrefabData(m_targetPrefab);
                FormWindowData.CurrentPrefabCacheData = m_cacheData;
                if (m_cacheData is null)
                {
                    m_targetPrefab = null;
                }
            }

            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
            leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition);
            // 在这里渲染目录树
            if (FormWindowData.CurrentTreeView != null && m_targetPrefab != null)
            {
                FormWindowData.CurrentTreeView.OnGUI(new Rect(0, 0, position.width, position.height));
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 右列表
        /// </summary>
        private void DrawDrawRightWindow_ComponentsGUI()
        {
            if (m_targetPrefab == null) return;
            if (FormWindowData.CurrentTreeView.m_nowComponentsList == null) return;

            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true));

            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true));
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("UIClassName:", "UILoginFrom");
            if (GUILayout.Button("定位", GUILayout.Width(50), GUILayout.Height(20)))
            {
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            if (GUILayout.Button("   组件&事件配置", this[LayoutStyle.Title], GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }


            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true));
            for (int i = 0; i < FormWindowData.CurrentTreeView.m_nowComponentsList.Count; i++)
            {
                Component elementComp = FormWindowData.CurrentTreeView.m_nowComponentsList[i];
                string componentName =
                    EditorUtils.GetComponentNameByType(elementComp);

                Color color = FormWindowData.CurrentTreeView.m_nowClikComponent.GetType() ==
                              elementComp.GetType()
                    ? Color.green
                    : Color.white;
                GUI.backgroundColor = color;
                int mInstanceId = FormWindowData.CurrentTreeView.m_nowComponentsList[i].GetInstanceID();
                string strComp = $"{componentName}-{mInstanceId}";
                if (GUILayout.Button(strComp,
                        GUILayout.Height(23)))
                {
                    OnClikComponent(FormWindowData.CurrentTreeView.m_nowComponentsList[i]);
                }

                GUI.backgroundColor = Color.white;
            }

            EditorGUILayout.EndVertical();
            //======================================================
            centerEventScrollPosition = EditorGUILayout.BeginScrollView(centerEventScrollPosition);
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            if (FormWindowData.CurrentTreeView.m_nowClikComponent != null)
            {
                Component ompenent = FormWindowData.CurrentTreeView.m_nowClikComponent;
                DrawEventList(ompenent);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 点击组件,更新事件列表
        /// </summary>
        private void OnClikComponent(Component component)
        {
            //更新事件列表
            FormWindowData.CurrentTreeView.m_nowClikComponent = component;
        }

        /// <summary>
        /// 绘制事件列表
        /// </summary>
        /// <param name="id"></param>
        private void DrawEventList(Component component)
        {
            SOComponent targetCompData = FormWindowData.CurrentTreeView.GetCurrentClikSoComponentData();
            if (targetCompData is null) return;
            //判断组件数据是否已经存在，存在则取存在信息，不存在则创建新的
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            //Title
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("   事件类型", this[LayoutStyle.Title], GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }

            if (GUILayout.Button("   函数/字段别名", this[LayoutStyle.Title], GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }

            if (GUILayout.Button("    设置事件", this[LayoutStyle.Title], GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);
            int mlimit = 3;
            foreach (SOEvent eSoEvent in targetCompData.soEvents)
            {
                switch (eSoEvent.eventEnum)
                {
                    case SoEventEnum.RectTransform: //RectTransform
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[0] RectTransform",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        //=============================================================================================
                        break;
                    case SoEventEnum.Button: //=============================Button================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[1] Button",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            bool isMay = true;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.PointerClick ||
                                        VARIABLE.eventEnum == SoEventEnum.PointerDown ||
                                        VARIABLE.eventEnum == SoEventEnum.PointerUp)
                                    {
                                        if (VARIABLE.isSetup)
                                        {
                                            isMay = false;
                                            EditorUtility.DisplayDialog("警告",
                                                $"PointerClick/PointerDown/PointerUp 必须需要Button组件引用!",
                                                "确定");
                                        }
                                    }
                                }
                            }

                            if (isMay)
                            {
                                eSoEvent.isSetup = !eSoEvent.isSetup;
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;


                        break;
                    case SoEventEnum.Text: //=============================Text================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[1] Text",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.Image: //=============================Image================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[1] Image",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.Slider: //=============================Slider================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[1] Slider",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            bool isMay = true;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.SliderOnValueChanged)
                                    {
                                        if (VARIABLE.isSetup)
                                        {
                                            isMay = false;
                                            EditorUtility.DisplayDialog("警告", $"OnValueChanged 必须需要Slider组件引用!",
                                                "确定");
                                        }
                                    }
                                }
                            }

                            if (isMay)
                            {
                                eSoEvent.isSetup = !eSoEvent.isSetup;
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;

                        break;
                    case SoEventEnum.Toggle: //=============================Toggle================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[1] Toggle",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            bool isMay = true;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.ToggleOnValueChanged)
                                    {
                                        if (VARIABLE.isSetup)
                                        {
                                            isMay = false;
                                            EditorUtility.DisplayDialog("警告", $"OnValueChanged 必须需要Toggle组件引用!",
                                                "确定");
                                        }
                                    }
                                }
                            }

                            if (isMay)
                            {
                                eSoEvent.isSetup = !eSoEvent.isSetup;
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;

                        break;
                    case SoEventEnum.PointerClick
                        : //=============================PointerClick================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[2] PointerClick",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.Button)
                                    {
                                        VARIABLE.isSetup = eSoEvent.isSetup;
                                    }
                                }
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;

                        break;
                    case SoEventEnum.PointerDown
                        : //=============================PointerDown================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[3] PointerDown",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.Button)
                                    {
                                        VARIABLE.isSetup = eSoEvent.isSetup;
                                    }
                                }
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.PointerUp: //=============================PointerUp================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[4] PointerUp",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.Button)
                                    {
                                        VARIABLE.isSetup = eSoEvent.isSetup;
                                    }
                                }
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.PointerEnter
                        : //=============================PointerEnter================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[5] PointerEnter",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.PointerExit
                        : //=============================PointerExit================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[6] PointerExit",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.SliderOnValueChanged
                        : //=============================OnValueChanged================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[2] OnValueChanged",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.Slider)
                                    {
                                        VARIABLE.isSetup = eSoEvent.isSetup;
                                    }
                                }
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                    case SoEventEnum.ToggleOnValueChanged
                        : //=============================OnValueChanged================================
                        GUILayout.Space(mlimit);
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
                        GUILayout.Space(8);
                        EditorGUILayout.LabelField("[2] OnValueChanged",
                            this[15, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white], GUILayout.Height(12),
                            GUILayout.ExpandHeight(true));
                        GUILayout.Space(8);
                        eSoEvent.mehtodName = EditorGUILayout.TextField(
                            eSoEvent.mehtodName,
                            GUILayout.ExpandHeight(true));
                        GUI.backgroundColor = eSoEvent.isSetup ? Color.red : Color.green;
                        if (GUILayout.Button(eSoEvent.isSetup ? "取消设置" : "设置", GUILayout.ExpandHeight(true)))
                        {
                            eSoEvent.isSetup = !eSoEvent.isSetup;
                            if (eSoEvent.isSetup)
                            {
                                foreach (var VARIABLE in targetCompData.soEvents)
                                {
                                    if (VARIABLE.eventEnum == SoEventEnum.Toggle)
                                    {
                                        VARIABLE.isSetup = eSoEvent.isSetup;
                                    }
                                }
                            }
                        }

                        GUI.backgroundColor = eSoEvent.isSetup ? Color.green : Color.white;
                        EditorGUILayout.EndHorizontal();
                        GUI.backgroundColor = Color.white;
                        break;
                }
            }

            EditorGUILayout.EndVertical();

            if (GUI.changed)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 顶部按钮
        /// </summary>
        private void DrawTopMenuBtnsGUI()
        {
            EditorGUILayout.BeginHorizontal();

            GUI.backgroundColor = FormWindowData.windowArea == WindowArea.Base ? Color.green : Color.white;
            if (GUILayout.Button("基础配置", GUILayout.Height(23)))
            {
                FormWindowData.windowArea = WindowArea.Base;
            }

            GUI.backgroundColor = Color.white;
            GUI.backgroundColor = FormWindowData.windowArea == WindowArea.Prefab ? Color.green : Color.white;
            if (GUILayout.Button("预制件配置", GUILayout.Height(23)))
            {
                FormWindowData.windowArea = WindowArea.Prefab;
            }

            GUI.backgroundColor = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        /// <summary>
        /// 绘制基本信息界面
        /// </summary>
        private void DrawDirectory()
        {
            GUILayout.Space(5);
            GUILayout.Label("当前目录[生成的C# UI代码存放目录]:");
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);


            GUILayout.Label(string.IsNullOrEmpty(FormWindowData.WindowData.generateParentDirectoryPath)
                ? "空目录"
                : FormWindowData.WindowData.generateParentDirectoryPath);

            if (GUILayout.Button("选择目录", GUILayout.Height(20)))
            {
                FormWindowData.WindowData.generateParentDirectoryPath =
                    EditorUtility.OpenFolderPanel("选择目录", Application.dataPath, "");
            }

            EditorGUILayout.EndVertical();


            if (!string.IsNullOrEmpty(FormWindowData.WindowData.generateParentDirectoryPath))
            {
                //检查路径合法?
                if (FormWindowData.WindowData.generateParentDirectoryPath.Contains("Resources") ||
                    FormWindowData.WindowData.generateParentDirectoryPath.Contains("StreamingAssets") ||
                    FormWindowData.WindowData.generateParentDirectoryPath.Contains("Plugins") ||
                    FormWindowData.WindowData.generateParentDirectoryPath.Contains("Editor")
                   )
                {
                    FormWindowData.WindowData.generateParentDirectoryPath = String.Empty;
                    EditorUtility.DisplayDialog("警告", "此路径包含不合法的路径Resources/StreamingAssets/Plugins/Editor，请重新选择路径!",
                        "确定");
                }
            }

            if (GUI.changed)
            {
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        /// <summary>
        /// 预制件映射
        /// </summary>
        private void DrawViewReflectionPrefab()
        {
            isFoldout = EditorGUILayout.Foldout(isFoldout,
                $"预制件映射列表:[{FormWindowData.ReflectionData.mFormDatas.Count}]");
            if (isFoldout)
            {
                centerReflectionScrollPosition = EditorGUILayout.BeginScrollView(centerReflectionScrollPosition);
                for (int i = 0; i < FormWindowData.ReflectionData.mFormDatas.Count; i++)
                {
                    EditorGUILayout.BeginVertical();

                    FormWindowData.ReflectionData.mFormDatas[i].mViewEnum =
                        (ViewEnum)EditorGUILayout.EnumPopup("ViewEnum",
                            FormWindowData.ReflectionData.mFormDatas[i].mViewEnum);
                    FormWindowData.ReflectionData.mFormDatas[i].mABName =
                        EditorGUILayout.TextField("ABName", FormWindowData.ReflectionData.mFormDatas[i].mABName);
                    FormWindowData.ReflectionData.mFormDatas[i].mAssetName =
                        EditorGUILayout.TextField("AssetName", FormWindowData.ReflectionData.mFormDatas[i].mAssetName);
                    if (GUILayout.Button("X"))
                    {
                        FormWindowData.ReflectionData.mFormDatas.RemoveAt(i);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                    }

                    EditorGUILayout.EndVertical();
                }

                if (GUILayout.Button("+", GUILayout.Height(30)))
                {
                    FormWindowData.ReflectionData.mFormDatas.Add(new FormData());
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }

                EditorGUILayout.EndScrollView();
            }
        }

        /// <summary>
        /// 模块样式索引
        /// </summary>
        /// <param name="ls"></param>
        string this[LayoutStyle ls]
        {
            get
            {
                string str = "";
                switch (ls)
                {
                    case LayoutStyle.Title:
                        str = "MeTransOffRight";
                        break;
                    case LayoutStyle.AddBtn:
                        str = "CN CountBadge";
                        break;
                    case LayoutStyle.GroupBox:
                        str = "GroupBox";
                        break;
                    case LayoutStyle.HelpBox:
                        str = "HelpBox";
                        break;
                    case LayoutStyle.FoldOut:
                        str = "Foldout";
                        break;
                    case LayoutStyle.Toggle:
                        str = "BoldToggle";
                        break;
                }

                return str;
            }
        }

        public enum LayoutStyle
        {
            Title,
            AddBtn,
            GroupBox,
            HelpBox,
            FoldOut,
            Toggle,
        }

        /// <summary>
        /// 文字样式索引
        /// </summary>
        /// <param name="size"></param>
        /// <param name="fontStyle"></param>
        /// <param name="anchor"></param>
        /// <param name="color"></param>
        GUIStyle this[int size, FontStyle fontStyle, TextAnchor anchor, Color color]
        {
            get
            {
                GUIStyle _style = new GUIStyle();
                _style.fontSize = size;
                _style.fontStyle = fontStyle;
                _style.alignment = anchor;
                _style.normal.textColor = color;
                return _style;
            }
        }
    }
}