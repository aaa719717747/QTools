using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Config
{
    public class PrefabReflectionData
    {
        public static List<EnumForm> EnumForms = new List<EnumForm>();
    }

    [System.Serializable]
    public class EnumForm
    {
        public ViewEnum ViewEnum;
        public Form form;
    }
}