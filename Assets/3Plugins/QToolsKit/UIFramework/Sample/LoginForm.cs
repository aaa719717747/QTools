using _3Plugins.QToolsKit.UIFramework.Scripts;

namespace _3Plugins.QToolsKit.UIFramework.Sample
{
    public class LoginForm : ViewAbstract<LoginView, LoginModel>
    {
        public override LoginView View { get; set; }
        public override LoginModel Model { get; set; }
        
    }
}