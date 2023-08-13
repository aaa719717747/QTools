using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using Unity.Plastic.Newtonsoft.Json;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Utils
{
    public class JsonUtils
    {
        public static void Get<T>(T t)
        {
            if (t is PrefabCacheData)
            {
               
            }
            if (t is GlobalUIWindowData)
            {
            }
        }
        public static void Write<T>(T t)
        {
            if (t is PrefabCacheData)
            {
                JsonConvert.SerializeObject(FormWindowData.PrefabData);
            }
            if (t is GlobalUIWindowData)
            {
                JsonConvert.SerializeObject(FormWindowData.WindowData);
            }
        }
    }
}