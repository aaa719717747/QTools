using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Scripts;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _3Plugins.QToolsKit.UIFramework.Sample.Login
{
    public class LoginPanel : QUIForm, IForm<LoginBindView, LoginCtrl>
    {
        public LoginBindView View { get; set; }
        public LoginCtrl Ctrl { get; set; }
        public override void Init()
        {
            View = new LoginBindView();
            Ctrl = new LoginCtrl();
            View.MTransform = transform;
            View.OnInitBind(
                OnClik_MLoginBtn,
                            OnClik_MLoginBtn2
            );
            Ctrl.OnInit();
        }

        public void OnClik_MLoginBtn()
        {
        }

        public void OnClik_MLoginBtn2()
        {
        }

        public override void OnPrepareLoadData()
        {
        }

        public override void OnEverythingReady()
        {
        }

        public override void RegisterGlobalEvent()
        {
            Ctrl.RegisterGlobalEvent();
        }

        public override void UnRegisterGlobalEvent()
        {
            Ctrl.UnRegisterGlobalEvent();
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