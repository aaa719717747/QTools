using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DKit.Modules.Protobuf.Runtime.SDK;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace DKit.Modules.Protobuf.Editor
{
    public static class ProtobufUtil
    {
        private static PbConfig _pbInitData;
        static PbConfig PbInitData
        {
            get
            {
                if (_pbInitData == null)
                {
                    var data = AssetDatabase.LoadAssetAtPath<PbConfig>(
                        "Assets/DKit/Modules/Protobuf/Runtime/SDK/Resources/PBConfig.asset");
                    if (data)
                    {
                        _pbInitData = data;
                    }
                    else
                    {
                        PbConfig n_data = ScriptableObject.CreateInstance<PbConfig>();
                        AssetDatabase.CreateAsset(n_data,
                            "Assets/DKit/Modules/Protobuf/Runtime/SDK/Resources/PBConfig.asset");
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        _pbInitData = n_data;
                    }
                }
                return _pbInitData;
            }
            
        }
        static string Path
        {
            get
            {
                string strPath = String.Empty;
                switch (PbInitData.pbPathType)
                {
                    case PBPathType.persistentDataPath:
                        strPath = Application.persistentDataPath;
                        break;
                    case PBPathType.dataPath:
                        strPath = Application.dataPath;
                        break;
                    case PBPathType.streamingAssetsPath:
                        strPath = Application.streamingAssetsPath;
                        break;
                    case PBPathType.custom:
                        strPath = PbInitData.customPath;
                        break;
                }

                return strPath + "/Infos";
            }
        }
        
       

        [MenuItem("DKit/PB/打开Pb文件目录", false, 3)]
        public static void OpenPbDirectory()
        {
            if (Directory.Exists(Path))
            {
                Execute(Path);
            }
            else
            {
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
            }
        }

        [MenuItem("DKit/PB/清除Pb目录", false, 4)]
        public static void ClearPbDirectory()
        {
            ClearPBFile();
        }

        [MenuItem("DKit/PB/更新Pb密钥(谨慎)", false, 5)]
        public static void UpdatePbPasswordKey()
        {
            if (string.IsNullOrEmpty(PbInitData.pkey))
            {
                PbInitData.pkey = GUID.Generate().ToString();
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示!", "更新密钥会删除之前的所有PB文件，你确定?", "OK", "Cancel"))
                {
                    PbInitData.pkey = GUID.Generate().ToString();
                    ClearPBFile();
                }
            }
        }

        /// <summary>
        /// 清除PB目录
        /// </summary>
        /// <exception cref="Exception"></exception>
        private static void ClearPBFile()
        {
            int a = 0;
            try
            {
                string[] files = Directory.GetFiles(Path); //得到文件
                foreach (string file in files) //循环文件
                {
                    string exname = file.Substring(file.LastIndexOf(".") + 1); //得到后缀名
                    if ($".{PbInitData.suffix}".IndexOf(file.Substring(file.LastIndexOf(".") + 1)) > -1) //如果后缀名为.txt文件
                    {
                        Debug.Log(file);
                        FileInfo fi = new FileInfo(file); //建立FileInfo对象
                        File.Delete(file);
                        a++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            Debug.Log($"清除了{a}个文件！");
        }

        /// <summary>
        /// 打开指定路径的文件夹。
        /// </summary>
        /// <param name="folder">要打开的文件夹的路径。</param>
        public static void Execute(string folder)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;

                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;

                default:
                    throw new FileNotFoundException($"Not support open folder on '{Application.platform}' platform.");
            }
        }
    }
}