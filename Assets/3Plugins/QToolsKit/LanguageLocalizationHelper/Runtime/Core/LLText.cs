using UnityEngine;
using UnityEngine.UI;

namespace DKit.Modules.LanguageLocalizationHelper.Runtime.Core
{
    [RequireComponent(typeof(Text))]
    public class LLText : LLBaseObject
    {
        private Text content;

        public Text Content
        {
            get
            {
                if (content == null)
                {
                    return GetComponent<Text>();
                }

                return content;
            }
        }

        public override void UpdateContent(LLISO iso)
        {
            Content.text = base.GetTargetScheme(base.GetLlObject, iso).content;
        }
    }
}
