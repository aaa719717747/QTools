using System;
using System.IO;
using _3Plugins.QToolsKit.UIFramework.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Utils
{
    public static class EditorUtils
    {
        /// <summary>
        /// 生成新的Form模块
        /// </summary>
        public static void GenerateNewForm(string formName)
        {
            //应该先检查删除原文件夹
            //加载模板

            string pathView = $"3Plugins/QToolsKit/UIFramework/Editor/TemplateAssets/ViewTemplate.txt";
            string viewTxt = Path.Combine(Application.dataPath, pathView);
            string pathModel = $"3Plugins/QToolsKit/UIFramework/Editor/TemplateAssets/ModelTemplate.txt";
            string modelTxt = Path.Combine(Application.dataPath, pathModel);
            string pathForm = $"3Plugins/QToolsKit/UIFramework/Editor/TemplateAssets/FormTemplate.txt";
            string formTxt = Path.Combine(Application.dataPath, pathForm);
            TextAsset viewTemp = AssetDatabase.LoadAssetAtPath<TextAsset>(viewTxt);
            TextAsset modelTemp = AssetDatabase.LoadAssetAtPath<TextAsset>(modelTxt);
            TextAsset formTemp = AssetDatabase.LoadAssetAtPath<TextAsset>(formTxt);
            string scriptContent = viewTemp.text.Replace("#类名#", formName);
            string scriptPath = EditorUtility.SaveFilePanel("Save Script", "Assets", $"{formName}View" + ".cs", "cs");

            // if (!string.IsNullOrEmpty(scriptPath))
            // {
            //     FileUtil.WriteTextToFile(scriptPath, scriptContent);
            //     AssetDatabase.Refresh();
            //     Debug.Log("Script generated successfully at path: " + scriptPath);
            // }
        }


        public static string GetComponentNameByType(Component component)
        {
            string comName = String.Empty;
            if (component is Button)
            {
                comName = "Button";
            }
            else if (component is Text)
            {
                comName = "Text";
            }
            else if (component is Canvas)
            {
                comName = "Canvas";
            }
            else if (component is Image)
            {
                comName = "Image";
            }
            else if (component is ScrollRect)
            {
                comName = "ScrollRect";
            }
            else if (component is Slider)
            {
                comName = "Slider";
            }
            else if (component is Toggle)
            {
                comName = "Toggle";
            }
            else if (component is QUIForm)
            {
                comName = "QUIForm";
            }

            return comName;
        }

        /// <summary>
        /// pathView路径仅限于Assets/目录下
        /// 例如:Assets/xxxx.asset
        /// </summary>
        /// <param name="pathView"></param>
        /// <typeparam name="T"></typeparam>
        public static T CreateScriptableObject<T>(string pathView) where T : ScriptableObject
        {
            var exampleAsset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(exampleAsset, pathView);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return exampleAsset;
        }

        public static bool IsHavaTypeImageComponent(Component[] components)
        {
            foreach (Component VARIABLE in components)
            {
                if (VARIABLE is Image)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsHavaTypeTextComponent(Component[] components)
        {
            foreach (Component VARIABLE in components)
            {
                if (VARIABLE is Text)
                {
                    return true;
                }
            }

            return false;
        }
    }
}