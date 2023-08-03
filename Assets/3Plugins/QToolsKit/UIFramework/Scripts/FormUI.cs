using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts
{
    public class FormUI: ViewAbstract<BaseView, BaseModel>
    {
        public override BaseView View { get; set; }
        public override BaseModel Model { get; set; }

        public override void OnShow()
        {
            Debug.Log("FormUI  is  show!");
        }

        public override void OnClose()
        {
        }
    }
}