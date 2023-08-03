using System.IO;
using _3Plugins.QToolsKit.UIFramework.Editor.Configs;
using _3Plugins.QToolsKit.UIFramework.Editor.Configs.Scripts;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window
{
    public class EditorFormWindow : EditorWindow
    {
        public static UIFormEditorConfig FormEditorConfig;

        [MenuItem("编辑器工具/UI编辑器")]
        public static void ShowWindow()
        {
            WindowInit();
            EditorWindow.GetWindow(typeof(EditorFormWindow), false, "UI编辑器");
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
                GUILayout.Space(5);
                GUILayout.Label("当前目录[生成的C# UI代码存放目录]:" + FormEditorConfig.generateParentDirectoryPath);

                if (GUILayout.Button("选择目录"))
                {
                    FormEditorConfig.generateParentDirectoryPath =
                        EditorUtility.OpenFolderPanel("选择目录", Application.dataPath, "");
                }
            }
        }
    }
}