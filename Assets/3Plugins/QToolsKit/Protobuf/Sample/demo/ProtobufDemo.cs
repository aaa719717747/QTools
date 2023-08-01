using System;
using System.Collections.Generic;
using DKit.Modules.Protobuf.Runtime.SDK;
using Google.Protobuf;
using Im.Unity.protobuf_unity.Tests.Runtime.ProtoDemo;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace DKit.Modules.Protobuf.Sample.demo
{
    public class ProtobufDemo : MonoBehaviour
    {
        Dictionary<Type, IMessage> pbDataDicts =
            new Dictionary<Type, IMessage>();

        public string userName;


        /// <summary>
        ///  默认初始化pb数据在这里设置
        /// </summary>
        public void Init()
        {
            pbDataDicts.Add(typeof(DemoUser), 
                new DemoUser
            {
                Id = 1,
                Name = "默认名称",
                RoleId = "1000",
                RoleArrays = { 1, 2, 3 },
                LoginReq = new LoginRequest
                {
                    Account = "admin",
                    Password = "**********",
                    Dict =
                    {
                        {1,"aaa"}
                    }
                },
                IsOk = false,
                Corpus = Corpus.Video,
                A = 0.99f
            });
            
            pbDataDicts.Add(typeof(DemoSetting),new DemoSetting
            {
                Id = 1,
                A = 0.3335f
            });
        }

        public T Get<T>() where T : IMessage
        {
            return (T)pbDataDicts[typeof(T)];
        }

        private void Awake()
        {
            Init();
            ProtobufHelper.Read(pbDataDicts[typeof(DemoUser)]);
            ProtobufHelper.Read(pbDataDicts[typeof(DemoSetting)]);
            userName = Get<DemoUser>().Name;
        }

        private void OnGUI()
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 23;
            if (GUI.Button(new Rect(200, 200, 100, 50), "取值", guiStyle))
            {
                Debug.Log(Get<DemoUser>().Name);
            }

            if (GUI.Button(new Rect(200, 250, 100, 50), "修改", guiStyle))
            {
                Get<DemoUser>().Name = userName;
            }

            if (GUI.Button(new Rect(200, 300, 100, 50), "保存", guiStyle))
            {
                foreach (var msg in pbDataDicts)
                {
                    ProtobufHelper.Write(msg.Value);
                }
            }
        }
    }
}