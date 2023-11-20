using System;
using _3Plugins.QToolsKit.Loader;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    [DisallowMultipleComponent]
    public abstract class QUIForm : MonoBehaviour
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

        /// <summary>
        /// 注册全局事件
        /// </summary>
        public abstract void RegisterGlobalEvent();

        /// <summary>
        /// 注销全局事件
        /// </summary>
        public abstract void UnRegisterGlobalEvent();

        /// <summary>
        /// UI数据结构初始化环节
        /// 1.ViewBind层绑定UIBehaviour事件监听，获取组件路径引用
        /// 2.Ctrl层网络相关请求与回包注册
        /// </summary>
        public abstract void Init();

        /// <summary>
        /// 开始准备数据阶段
        /// </summary>
        public abstract void OnPrepareLoadData();

        /// <summary>
        /// [一切数据就绪]数据准备完毕后调用
        /// </summary>
        public abstract void OnEverythingReady();

        public abstract void OnOpen(UserTB userData = null);
        public abstract void OnClose();


        public virtual void Close()
        {
            OnClose();
        }
    }
}