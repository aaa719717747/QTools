using System.Collections.Generic;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Scripts.Core
{
    public class UIMgr: MonoBehaviour
    {
        private Dictionary<ViewEnum, Form> formDatas= new Dictionary<ViewEnum, Form>();

        public void Init()
        {
            
        }

        public void OpenForm(ViewEnum viewEnum, UserTB userData = null)
        {
            GameObject prefab=formDatas[viewEnum].GetPrefab();
            GameObject go=Instantiate(prefab,this.transform);
            go.transform.SetAsLastSibling();
        }
    }
}