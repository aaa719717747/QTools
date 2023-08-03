using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts
{
    public abstract class UIForm<TView, TModel> : MonoBehaviour
        where TView : BaseView
        where TModel : BaseModel
    {
        public ViewEnum viewEnum;

        public void Bind(TView view, TModel model)
        {
            View = view;
            Model = model;
        }

        public abstract TView View { get; set; }
        public abstract TModel Model { get; set; }

        public abstract void OnShow();
        public abstract void OnClose();
    }
}