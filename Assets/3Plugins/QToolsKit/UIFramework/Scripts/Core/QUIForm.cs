using System;
using _3Plugins.QToolsKit.Loader;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    [DisallowMultipleComponent]
    public abstract class QUIForm: MonoBehaviour
    {
        /*
        * 1.调整分辨率
        * 2.UI资源预加载信息
        * 3.层级相关
        * 4.优先级
        * 5.出入场动画
        * 6.编辑器功能【定位到Prefab】
        */
        public ViewEnum mViewEnum;
       
        public abstract void RegisterGlobalEvent();
        public abstract void OnOpen(UserTB userData = null);
        public abstract void OnClose();

        public virtual void Close()
        {
            OnClose();
        }
    }
}