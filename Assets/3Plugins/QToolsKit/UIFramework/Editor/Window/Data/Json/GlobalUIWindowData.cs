using System.Collections.Generic;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json
{
    [System.Serializable]
    public class GlobalUIWindowData
    {
        [Header("生成UIFrom代码的存放父级目录")]
        public string generateParentDirectoryPath;
        public List<PrefabData> mPrefabsCacheDatas = new List<PrefabData>();
    }

    [System.Serializable]
    public class PrefabData
    {
        public int mInstanceId;
        public PrefabCacheData mCacheData;
    }
}