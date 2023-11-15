namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public abstract class PanelBase
    {
        public abstract void OnOpen(UserTB userData = null);
        public abstract void OnClose();
        public abstract void RegisterGlobalEvent();

        public virtual void Close()
        {
            OnClose();
        }
    }
}