using System;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window
{
    public class PrefabTreeViewWindow : EditorWindow
    {
        [SerializeField] TreeViewState m_TreeViewState;

        PrefabTreeView m_SimpleTreeView;
        private Vector2 leftScrollPosition;
        private Vector2 treeviewScrollPosition;
        private Vector2 rightScrollPosition;

        private string searchQuery = "";
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
            // Get existing open window or if none, make a new one:
            var window = GetWindow<PrefabTreeViewWindow>();
            window.titleContent = new GUIContent("My Window");
            window.Show();
        }

        void OnEnable()
        {
            // Check whether there is already a serialized view state (state 
            // that survived assembly reloading)
            if (m_TreeViewState == null)
                m_TreeViewState = new TreeViewState();

            GameObject preab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/RegisterForm.prefab");
            m_SimpleTreeView = new PrefabTreeView(m_TreeViewState, preab);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();

            // 左侧窗口
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.Width(position.width / 3));
            // leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition);
            // EditorGUILayout.ObjectField()
            if (GUILayout.Button("定位", GUILayout.Height(20)))
            {
                Debug.Log("Button 1 clicked");
            }

            EditorGUILayout.LabelField("Search Field Example", EditorStyles.boldLabel);

            // 绘制搜索框
            // searchQuery = EditorGUILayout.ToolbarSearchField(searchQuery);

            // 绘制搜索框
            // searchQuery = EditorGUI.SearchField(new Rect(100,100,100,100), searchQuery);
            // 显示搜索结果


            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox], GUILayout.Width(position.width / 3));
            treeviewScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition);
            // 在这里渲染目录树
            if (m_SimpleTreeView != null)
            {
                m_SimpleTreeView.OnGUI(new Rect(0, 0, position.width / 3, position.height));
            }

            // EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();


            // 右侧窗口
            EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox],
                GUILayout.Width(position.width - (position.width / 3)));
            rightScrollPosition = EditorGUILayout.BeginScrollView(rightScrollPosition);
            // 在这里渲染按钮和文本区域
            EditorGUILayout.LabelField("控件&事件绑定");

            //渲染所有组件
            RendererComponents();

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private bool isSelect;

        private void RendererComponents()
        {
            for (int i = 0; i < FormWindowData.currentNodeComponents.Count; i++)
            {
                Component _type = FormWindowData.currentNodeComponents[i];
                string componmentName = String.Empty;
                if (_type is RectTransform)
                {
                    componmentName = "RectTransform";
                }
                else if (_type is Image)
                {
                    componmentName = "Image";
                }
                else if (_type is Button)
                {
                    componmentName = "Button";
                }
                else if (_type is Text)
                {
                    componmentName = "Text";
                }
                else if (_type is Toggle)
                {
                    componmentName = "Toggle";
                }
                else if (_type is ScrollRect)
                {
                    componmentName = "ScrollRect";
                }
                else if (_type is Slider)
                {
                    componmentName = "Slider";
                } else if (_type is InputField)
                {
                    componmentName = "InputField";
                }

                if (!string.IsNullOrEmpty(componmentName))
                {
                    GUI.backgroundColor = Color.yellow;
                    EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

// LabelField
                    EditorGUILayout.LabelField(componmentName, GUILayout.ExpandWidth(true));

// TextField    
                    EditorGUILayout.TextField("Btn_rf", GUILayout.ExpandWidth(true));

// Toggle
                    GUI.backgroundColor = Color.yellow;
                    isSelect = EditorGUILayout.Toggle(isSelect);
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
            }
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