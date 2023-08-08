using System.Collections.Generic;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public  class FormWindowData
    {
        [Header("生成UIFrom代码的存放父级目录")]
        public string generateParentDirectoryPath;
        public static List<Component> currentNodeComponents = new List<Component>();

        public static void UpdateNodeComponentsList(List<Component> components)
        {
            currentNodeComponents.Clear();
            currentNodeComponents = components;
        }
    }
}