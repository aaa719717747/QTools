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
            else if (component is QUIFormScript)
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