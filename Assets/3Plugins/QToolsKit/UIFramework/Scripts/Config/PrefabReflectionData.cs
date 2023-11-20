using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Config
{
   [CreateAssetMenu(fileName = "PrefabReflectionData",menuName = "PrefabReflectionData(ScriptableObject)")]
    public class PrefabReflectionData: ScriptableObject
    {
        public List<FormData> mFormDatas = new List<FormData>();

        public FormData GetFormData(ViewEnum mViewEnum)
        {
            foreach (var variable in mFormDatas)
            {
                if (variable.mViewEnum == mViewEnum)
                {
                    return variable;
                }
            }

            return null;
        }
    }

    [System.Serializable]
    public class FormData
    {
        public ViewEnum mViewEnum;
        public string mABName;
        public string mAssetName;
    }
}