﻿using System;
using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
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
            FormWindowData.Init();
            var window = GetWindow<PrefabTreeViewWindow>();
            window.titleContent = new GUIContent("UI编辑器");
            nowInstanceId = 0;
            window.Show();
        }


        private void OnGUI()
        {
            //顶部按钮
            DrawTopMenuBtnsGUI();
            //==========================基础配置=========================

            //==========================预制件配置=========================
            EditorGUILayout.BeginHorizontal();
            // 左侧窗口
            DrawLeftTreeViewWindowGUI();
            // 右侧窗口
            DrawDrawRightWindow_ComponentsGUI();
            EditorGUILayout.EndHorizontal();
        }

        private static int nowInstanceId;

        /// <summary>
        /// 左侧目录树
        /// </summary>
        private void DrawLeftTreeViewWindowGUI()
        {
            // 左侧窗口
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
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
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true));
            if (GUILayout.Button("   组件&事件配置", this[LayoutStyle.Title], GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }
            // EditorGUILayout.LabelField("组件&事件配置",GUILayout.Height(20),GUILayout.ExpandWidth(true));

            if (m_targetPrefab != null)
            {
                EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true));
                if (FormWindowData.CurrentTreeView.m_nowComponentsList != null)
                {
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
                        if (GUILayout.Button(componentName,
                                GUILayout.Height(23)))
                        {
                            OnClikComponent(FormWindowData.CurrentTreeView.m_nowComponentsList[i]);
                        }

                        GUI.backgroundColor = Color.white;
                    }
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
            }

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
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(true));

            //Title
            EditorGUILayout.BeginHorizontal();
            GUI.backgroundColor=Color.yellow;
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
            GUI.backgroundColor=Color.white;
            EditorGUILayout.EndHorizontal();
            
            GUILayout.Space(20);
            //RectTransform
            EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
            GUILayout.Space(8);
            EditorGUILayout.LabelField("[0] RectTransform", GUILayout.Height(12),GUILayout.ExpandHeight(true));
            GUILayout.Space(8);
            EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_rf",GUILayout.ExpandHeight(true));
            if (GUILayout.Button("设置",GUILayout.ExpandHeight(true)))
            {
            }
            EditorGUILayout.EndHorizontal();
            
            
            
            
            
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal(this[LayoutStyle.Title]);
            
            EditorGUILayout.EndHorizontal();
            // //Button
            // if (component is Button)
            // {
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[1] PointerClick", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerClick",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[2] PointerDown", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerDown",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[3] PointerUp", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerUp",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[4] PointerEnter", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerEnter",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[5] PointerExit", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerExit",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            // }
            //
            // //Image
            // if (component is Image)
            // {
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[1] Image", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_img",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //     
            //     
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[2] PointerEnter", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerEnter",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            //
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("[3] PointerExit", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_PointerExit",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("取消设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            // }
            //
            // //Text
            // if (component is Text)
            // {
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("Text", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_txt",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            // }
            //
            // //Slider
            // if (component is Slider)
            // {
            //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            //     EditorGUILayout.LabelField("OnValueChanged", GUILayout.Height(12), GUILayout.Width(100));
            //     EditorGUILayout.TextField($"{FormWindowData.CurrentTreeView.m_nowClikNodeObj.name}_OnValueChanged",
            //         GUILayout.Width(150));
            //     if (GUILayout.Button("设置"))
            //     {
            //     }
            //
            //     EditorGUILayout.EndHorizontal();
            // }


            EditorGUILayout.EndVertical();
        }
        
        /// <summary>
        /// 顶部按钮
        /// </summary>
        private void DrawTopMenuBtnsGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("基础配置", GUILayout.Height(23)))
            {
                Debug.Log("Button 1 clicked");
            }

            if (GUILayout.Button("预制件配置", GUILayout.Height(23)))
            {
                Debug.Log("Button 1 clicked");
            }

            EditorGUILayout.EndHorizontal();
        }


        // private void DrawGenerateNewForm()
        // {
        //     if (!string.IsNullOrEmpty(FormEditorConfig.generateParentDirectoryPath))
        //     {
        //         //检查路径合法?
        //         if (FormEditorConfig.generateParentDirectoryPath.Contains("Resources") ||
        //             FormEditorConfig.generateParentDirectoryPath.Contains("StreamingAssets") ||
        //             FormEditorConfig.generateParentDirectoryPath.Contains("Plugins")
        //            )
        //         {
        //             FormEditorConfig.generateParentDirectoryPath = String.Empty;
        //             EditorUtility.DisplayDialog("警告", "此路径包含不合法的路径Resources/StreamingAssets/Plugins，请重新选择路径!",
        //                 "确定");
        //         }
        //         else
        //         {
        //             GUI.backgroundColor = Color.yellow;
        //             if (GUILayout.Button("生成C#代码", GUILayout.Height(35)))
        //             {
        //                 if (PrefabData.Prefab is null)
        //                 {
        //                     EditorUtility.DisplayDialog("警告", "UIPrefab预制件 空引用！请选择UIPrefab预制件！",
        //                         "确定");
        //                 }
        //             }
        //
        //             GUI.backgroundColor = Color.white;
        //         }
        //     }
        // }
        // private void DrawDirectory()
        // {
        //     isFoldoutDrawDirectory = EditorGUILayout.Foldout(isFoldoutDrawDirectory, "基础配置");
        //     if (isFoldoutDrawDirectory)
        //     {
        //         EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
        //         GUILayout.Space(5);
        //         GUILayout.Label("当前目录[生成的C# UI代码存放目录]:");
        //         GUILayout.Label(string.IsNullOrEmpty(FormEditorConfig.generateParentDirectoryPath)
        //             ? "空目录"
        //             : FormEditorConfig.generateParentDirectoryPath);
        //
        //         if (GUILayout.Button("选择目录", GUILayout.Height(20)))
        //         {
        //             FormEditorConfig.generateParentDirectoryPath =
        //                 EditorUtility.OpenFolderPanel("选择目录", Application.dataPath, "");
        //         }
        //
        //
        //         EditorGUILayout.EndVertical();
        //     }
        // }
        // private void DrawPrefabInfo()
        // {
        //     EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
        //     PrefabData.Prefab =
        //         (GameObject)EditorGUILayout.ObjectField("UIPrefab预制件:", PrefabData.Prefab, typeof(GameObject));
        //     EditorGUILayout.EndHorizontal();
        // }

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
    }
}