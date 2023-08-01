using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Google.Protobuf;
using UnityEngine;

namespace DKit.Modules.Protobuf.Runtime.SDK
{
    public static class ProtobufHelper
    {
        private static PbConfig _data;

        public static PbConfig Data
        {
            get
            {
                if (_data == null)
                {
                    return Resources.Load<PbConfig>("PBConfig");
                }

                return _data;
            }
        }


        #region 功能接口

        /// <summary>
        /// 写入存储protobuf
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        public static void Write<T>(T t) where T : IMessage
        {
            //全覆盖的存储属于安全存储
            File.WriteAllBytes($"{Path}/{t.GetType()}.{Data.suffix}",
                RijndaelEncrypt(t.ToByteArray(), Data.pkey));
        }

        /// <summary>
        /// 读取protobuf
        /// </summary>
        /// <param name="t"></param>
        /// <typeparam name="T"></typeparam>
        public static void Read<T>(T t) where T : IMessage
        {
            if (File.Exists($"{Path}/{t.GetType()}.{Data.suffix}"))
            {
                byte[] bs = File.ReadAllBytes($"{Path}/{t.GetType()}.{Data.suffix}");
                t.MergeFrom(RijndaelDecrypt(bs, Data.pkey));
            }
            else
            {
                Write(t);
            }
        }

        #endregion


        static string Path
        {
            get
            {
                string strPath = String.Empty;
                switch (Data.pbPathType)
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
                        strPath = Data.customPath;
                        break;
                }

                return strPath + "/Infos";
            }
        }


        #region 算法相关

        /// <summary>
        /// Rijndael加密算法
        /// </summary>
        /// <param name="pString">待加密的明文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        private static byte[] RijndaelEncrypt(byte[] toEncryptArray, string pkey)
        {
            //解密密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pkey);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();

            //返回加密后的密文
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return resultArray;
        }

        /// <summary>
        /// ijndael解密算法
        /// </summary>
        /// <param name="pString">待解密的密文</param>
        /// <param name="pKey">密钥,长度可以为:64位(byte[8]),128位(byte[16]),192位(byte[24]),256位(byte[32])</param>
        /// <param name="iv">iv向量,长度为128（byte[16])</param>
        /// <returns></returns>
        private static byte[] RijndaelDecrypt(byte[] toEncryptArray, string pKey)
        {
            //解密密钥
            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(pKey);

            //Rijndael解密算法
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            //返回解密后的明文
            byte[] resultArray = new byte[] { };
            try
            {
                resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            }
            catch (Exception e)
            {
                Debug.LogWarning("pb文件已损坏!,重置文档!");
            }

            return resultArray;
        }

        #endregion
    }
}