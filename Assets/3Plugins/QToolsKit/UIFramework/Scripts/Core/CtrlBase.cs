namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public abstract class CtrlBase
    {
        public abstract void OnInit();
        /// <summary>
        /// 注册全局事件
        /// </summary>
        public abstract void RegisterGlobalEvent();

        /// <summary>
        /// 注销全局事件
        /// </summary>
        public abstract void UnRegisterGlobalEvent();
    }
}