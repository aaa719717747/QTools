using System.IO;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using UnityEditor;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Export
{
    public  class ExportCodeUtil
    {
        public TextAsset s1;
        public static void Excute(PrefabCacheData data)
        {
            //文件路径
            string mUIClassName = FormWindowData.WindowData.generateParentDirectoryPath;
            string scriptPath = $"{mUIClassName}.cs";
            //上下文
            string context = "classname";

            FileStream file = new FileStream(scriptPath, FileMode.CreateNew);
            StreamWriter fileW = new StreamWriter(file, System.Text.Encoding.UTF8);
            fileW.Write(context);
            fileW.Flush();
            fileW.Close();
            file.Close();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}