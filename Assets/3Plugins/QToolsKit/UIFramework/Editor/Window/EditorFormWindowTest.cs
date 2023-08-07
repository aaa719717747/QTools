using System;
using System.IO;
using _3Plugins.QToolsKit.UIFramework.Editor.Configs;
using _3Plugins.QToolsKit.UIFramework.Editor.Configs.Scripts;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window
{
    public class EditorFormWindowTest : EditorWindow
    {
        public static UIFormEditorConfig FormEditorConfig;

        [FormerlySerializedAs("_isFoldout")] [SerializeField]
        private bool isFoldoutDrawDirectory = true;

        private Vector2 _scrollRectDrawDirectory;
        public static UIPrefabData PrefabData = new UIPrefabData();

        [FormerlySerializedAs("_isInMain")] [SerializeField]
        private bool isInMain = true;

        /*
            * 快捷键的写法：
               # 代表 shift  
               & 代表  alt
               % 代表 Ctrl
               之后加上 大写的字母 
            */

        [MenuItem("项目工具/UI编辑器 %&Q")]
        public static void ShowWindow()
        {
            WindowInit();
            EditorWindow.GetWindow(typeof(EditorFormWindowTest), false, "UI编辑器");
        }

        private static void WindowInit()
        {
            string pathView = "3Plugins/QToolsKit/UIFramework/Editor/Configs/UIFormEditorConfig.asset";
            string dataPath = Application.dataPath;
            string fullPath = Path.Combine(dataPath, pathView);
            string fullPathCreate = Path.Combine("Assets", pathView);
            if (File.Exists(fullPath))
            {
                FormEditorConfig = AssetDatabase.LoadAssetAtPath<UIFormEditorConfig>(fullPathCreate);
            }
            else
            {
                EditorUtils.CreateScriptableObject<UIFormEditorConfig>(fullPathCreate);
                FormEditorConfig = AssetDatabase.LoadAssetAtPath<UIFormEditorConfig>(fullPathCreate);
            }
        }

        private void OnGUI()
        {
            if (FormEditorConfig != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = isInMain ? Color.green : Color.white;
                if (GUILayout.Button("基础配置", GUILayout.Height(35)))
                {
                    isInMain = true;
                }

                GUI.backgroundColor = Color.white;
                GUI.backgroundColor = !isInMain ? Color.green : Color.white;
                if (GUILayout.Button("预制件配置", GUILayout.Height(35)))
                {
                    isInMain = false;
                }

                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
                if (isInMain)
                {
                    EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                    DrawDirectory();
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
                    DrawPrefabInfo();
                    DrawGenerateNewForm();
                    EditorGUILayout.EndHorizontal();
                }

                // EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox], GUILayout.Width(350));
                // DrawUIFormEditorArea();
                // EditorGUILayout.EndHorizontal();
            }
        }


        private void DrawUIFormEditorArea()
        {
            EditorGUILayout.TextArea(
                "000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000");
        }
        private void DrawGenerateNewForm()
        {
            if (!string.IsNullOrEmpty(FormEditorConfig.generateParentDirectoryPath))
            {
                //检查路径合法?
                if (FormEditorConfig.generateParentDirectoryPath.Contains("Resources") ||
                    FormEditorConfig.generateParentDirectoryPath.Contains("StreamingAssets") ||
                    FormEditorConfig.generateParentDirectoryPath.Contains("Plugins")
                   )
                {
                    FormEditorConfig.generateParentDirectoryPath = String.Empty;
                    EditorUtility.DisplayDialog("警告", "此路径包含不合法的路径Resources/StreamingAssets/Plugins，请重新选择路径!",
                        "确定");
                }
                else
                {
                    GUI.backgroundColor = Color.yellow;
                    if (GUILayout.Button("生成C#代码", GUILayout.Height(35)))
                    {
                        if (PrefabData.Prefab is null)
                        {
                            EditorUtility.DisplayDialog("警告", "UIPrefab预制件 空引用！请选择UIPrefab预制件！",
                                "确定");
                        }
                    }

                    GUI.backgroundColor = Color.white;
                }
            }
        }
        private void DrawDirectory()
        {
            isFoldoutDrawDirectory = EditorGUILayout.Foldout(isFoldoutDrawDirectory, "基础配置");
            if (isFoldoutDrawDirectory)
            {
                EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox]);
                GUILayout.Space(5);
                GUILayout.Label("当前目录[生成的C# UI代码存放目录]:");
                GUILayout.Label(string.IsNullOrEmpty(FormEditorConfig.generateParentDirectoryPath)
                    ? "空目录"
                    : FormEditorConfig.generateParentDirectoryPath);

                if (GUILayout.Button("选择目录", GUILayout.Height(20)))
                {
                    FormEditorConfig.generateParentDirectoryPath =
                        EditorUtility.OpenFolderPanel("选择目录", Application.dataPath, "");
                }


                EditorGUILayout.EndVertical();
            }
        }
        private void DrawPrefabInfo()
        {
            EditorGUILayout.BeginHorizontal(this[LayoutStyle.GroupBox]);
            PrefabData.Prefab =
                (GameObject)EditorGUILayout.ObjectField("UIPrefab预制件:", PrefabData.Prefab, typeof(GameObject));
            EditorGUILayout.EndHorizontal();
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