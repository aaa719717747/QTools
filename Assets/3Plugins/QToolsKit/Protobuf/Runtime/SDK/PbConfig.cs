using UnityEngine;

namespace DKit.Modules.Protobuf.Runtime.SDK
{
    public enum PBPathType
    {
        persistentDataPath,
        dataPath,
        streamingAssetsPath,
        custom
    }

    public class PbConfig : ScriptableObject
    {
        public PBPathType pbPathType;
        public string suffix;
        public string customPath;
        public string pkey;
    }
}