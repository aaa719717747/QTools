using QToolsKit.Loader;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts
{
    public class UIManager : MonoBehaviour
    {
        private GameObject m_maincitypanel;
        private GameObject m_sampleview;
        
        // [ItemCanBeNull] private Dictionary<ViewEnum, UIForm<,>> _forms = new Dictionary<ViewEnum, UIForm>();
        
        private void Start()
        {
            //初始化
            AssetBundleLoaderMgr.instance.Init();
            InitViewPanels();
           
        }


        // public T GetForm<T>(ViewEnum viewEnum)where T:UIForm
        // {
        //     return _forms[viewEnum] as T;
        // }

        public void InitViewPanels()
        {
            AssetBundleLoaderMgr.instance.Init();
            
            m_maincitypanel =
                AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/maincitypanel", "MainCityView");
            m_sampleview = AssetBundleLoaderMgr.instance.LoadAsset<GameObject>("ui/prefabs/sampleview", "SampleView");

            //实例化预设
            GameObject go = Instantiate(m_maincitypanel, transform);
            QUIFormScript uiForm = go.GetComponent<QUIFormScript>();
            // var ss= uiForm.Bind(
            //     new LoginView(),
            //     new LoginModel());
            // uiForm.viewEnum = ViewEnum.LOGIN;
            // _forms.Add(ViewEnum.LOGIN,ss);
        }
        
        public void ShowPanel(ViewEnum viewEnum)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                
                Instantiate(m_maincitypanel,transform);
                Instantiate(m_sampleview,transform);
            }
            // if (_forms.ContainsKey(viewEnum))
            // {
            //     _forms[viewEnum].OnShow();
            // }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("show"))
            {
                ShowPanel(ViewEnum.LoginForm);
            }
        }
    }
}