namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public interface IForm<TBindView, TCtrl>
        where TBindView : ViewBindBase
        where TCtrl : CtrlBase
    {
        public TBindView View { get; set; }
        public TCtrl Ctrl { get; set; }
    }
}