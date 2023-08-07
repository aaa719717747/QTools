using System.Collections.Generic;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    public static class FormWindowData
    {
        public static List<Component> currentNodeComponents = new List<Component>();

        public static void UpdateNodeComponentsList(List<Component> components)
        {
            currentNodeComponents.Clear();
            currentNodeComponents = components;
        }
    }
}