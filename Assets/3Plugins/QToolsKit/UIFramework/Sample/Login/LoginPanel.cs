using _3Plugins.QToolsKit.UIFramework.Scripts;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Sample.Login
{
    public class LoginPanel : QUIForm, IForm<LoginBindView, LoginCtrl>
    {
        public LoginBindView View { get; set; }
        public LoginCtrl Ctrl { get; set; }

        public override void RegisterGlobalEvent()
        {
        }

        public override void UnRegisterGlobalEvent()
        {
            
        }

        public override void OnInit()
        {
            View = new LoginBindView();
            Ctrl = new LoginCtrl();
        }

        public override void OnOpen(UserTB userData = null)
        {
            Debug.Log("打开LoginPanel");
        }

        public override void OnClose()
        {
        }
    }
}