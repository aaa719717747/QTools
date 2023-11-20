using System.ComponentModel;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Sample.Login
{
    public class LoginBindView : ViewBindBase
    {
        public Button MLoginBtn;
        public Text MLoginMessageText;
        public Slider MSlider;
        public override void OnInitBind(params UnityAction[] actions)
        {
            MLoginBtn.onClick.AddListener(actions[0]);
            // MSlider.onValueChanged.AddListener();
            MSlider = MTransform.Find("").GetComponent<Slider>();
            MLoginBtn = MTransform.Find("").GetComponent<Button>();
            MLoginMessageText = MTransform.Find("").GetComponent<Text>();
        }
    }
}