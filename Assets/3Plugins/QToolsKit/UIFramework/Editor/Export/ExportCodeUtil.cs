using System.IO;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using UnityEditor;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Export
{
    public class ExportCodeUtil
    {
        /// <summary>
        /// 生成新的CSharpForm模块
        /// </summary>
        public static void GenerateFormCSharpScript(string formName)
        {
            //应该先检查删除原文件夹
            //加载模板
        }

        /// <summary>
        /// 生成新的Lua模块
        /// </summary>
        public static void GenerateFormLuaScript(string formName)
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
    }
}