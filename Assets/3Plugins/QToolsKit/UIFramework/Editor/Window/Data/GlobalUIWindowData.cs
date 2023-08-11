using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data
{
    [CreateAssetMenu(fileName = "GlobalUIWindowData",menuName = "GlobalUIWindowData(ScriptableObject)")]
    public class GlobalUIWindowData: ScriptableObject
    {
        [Header("生成UIFrom代码的存放父级目录")]
        public string generateParentDirectoryPath;
       
    }
}