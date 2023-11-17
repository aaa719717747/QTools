using System.Collections.Generic;
using System.Linq;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data.Json;
using _3Plugins.QToolsKit.UIFramework.Scripts;
using _3Plugins.QToolsKit.UIFramework.Scripts.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView
{
    public class PrefabTreeView : UnityEditor.IMGUI.Controls.TreeView
    {
        public GameObject m_nowClikNodeObj;
        public Component m_nowClikComponent;
        public List<Component> m_nowComponentsList;


       

        public GameObject Prefab { get; set; }
        public List<TreeViewItem> AllItems { get; set; }

        public PrefabTreeView(TreeViewState treeViewState, GameObject prefab)
            : base(treeViewState)
        {
            Prefab = prefab;
            Reload();
        }
        /// <summary>
        /// 获取当前点击组件对应数据层的SOComponent数据
        /// </summary>
        /// <returns></returns>
        public SOComponent GetCurrentClikSoComponentData()
        {
            if (m_nowClikNodeObj is null)
            {
                Debug.LogError($"m_nowClikNodeObj is null");
                return null;
            }
            if (m_nowClikComponent is null)
            {
                Debug.LogError($"m_nowClikComponent is null");
                return null;
            }

            return FormWindowData.CurrentPrefabCacheData.GetCurrentClikSoComponentData(m_nowClikNodeObj,
                m_nowClikComponent);
        }
        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            List<TreeViewItem> treeViewItems = new List<TreeViewItem> { root };
            if (Prefab != null)
            {
                var topLevelNode = new TreeViewItem { id = 1, depth = 0, displayName = Prefab.name, parent = root };
                root.children = new List<TreeViewItem> { topLevelNode };
                treeViewItems.Add(topLevelNode);

                int id = 2;
                TraverseTransform(Prefab.transform, 1, ref treeViewItems, ref id, topLevelNode);
            }
            else
            {
                treeViewItems.Add(new TreeViewItem { id = 1, depth = 0, displayName = "No Prefab Selected" });
            }

            treeViewItems.RemoveAt(0);
            AllItems = treeViewItems;
            SingleClickedItem(0);
            return root;
        }

        private void TraverseTransform(Transform parent, int depth, ref List<TreeViewItem> items, ref int id,
            TreeViewItem parentNode = null)
        {
            foreach (Transform child in parent)
            {
                var item = new TreeViewItem { id = id++, depth = depth, displayName = child.name, parent = parentNode };
                items.Add(item);
                if (parentNode != null)
                {
                    parentNode.children = parentNode.children ?? new List<TreeViewItem>();
                    parentNode.children.Add(item);
                }

                TraverseTransform(child, depth + 1, ref items, ref id, item);
            }
        }

        protected override void RowGUI(RowGUIArgs args)
        {
            base.RowGUI(args);

            // 获取你想要显示的图标
            Texture2D iconText =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    "Assets/3Plugins/LanguageLocalizationHelper/Editor/Textures/text.jpg");
            Texture2D iconImage =
                AssetDatabase.LoadAssetAtPath<Texture2D>(
                    "Assets/3Plugins/LanguageLocalizationHelper/Editor/Textures/image.jpg");

            GameObject target = null;
            string path = GetGameObjectNameFromTreeViewItem(args.item);
            string rootName = Prefab.name;
            if (path.Equals(rootName))
            {
                target = Prefab.gameObject;
            }
            else
            {
                target = Prefab.transform.Find(path).gameObject;
            }

            Component[] components = target.GetComponents<Component>();

            int num = 0;
            if (EditorUtils.IsHavaTypeImageComponent(components))
            {
                num++;
                // 计算图标的位置
                Rect iconRect = new Rect(args.rowRect);
                iconRect.x += (150 + (num * 20)); // 10是图标和行末尾的间距
                iconRect.width = 15; //icon.width;
                iconRect.height = 15; //icon.height;
                // 绘制图标
                GUI.DrawTexture(iconRect, iconImage);
            }

            if (EditorUtils.IsHavaTypeTextComponent(components))
            {
                num++;
                // 计算图标的位置
                Rect iconRect = new Rect(args.rowRect);
                iconRect.x += (150 + (num * 20)); // 10是图标和行末尾的间距
                iconRect.width = 15; //icon.width;
                iconRect.height = 15; //icon.height;
                // 绘制图标
                GUI.DrawTexture(iconRect, iconText);
            }

            //测试
            int numTest = 0;
            Rect txtRect = new Rect(args.rowRect);
            txtRect.x += (330 + (numTest * 20)); // 10是图标和行末尾的间距
            txtRect.width = 23; //icon.width;
            txtRect.height = 15; //icon.height;
            GUI.TextField(txtRect,args.item.id.ToString());
            //测试
            int numTestInstance = 0;
            Rect txtInstanceIdRect = new Rect(args.rowRect);
            txtInstanceIdRect.x += (200 + (numTestInstance * 20)); // 10是图标和行末尾的间距
            txtInstanceIdRect.width = 60; //icon.width;
            txtInstanceIdRect.height = 15; //icon.height;
            GUI.TextField(txtInstanceIdRect,target.GetInstanceID().ToString());
        }

        /// <summary>
        /// 点击子叶节点事件
        /// </summary>
        /// <param name="id"></param>
        protected override void SingleClickedItem(int id)
        {
            FormWindowData.CurrentClikNodeId = id;
            GameObject target = null;
            base.SingleClickedItem(id);

            TreeViewItem clickedItem = FindItem(id, rootItem);

            if (clickedItem != null)
            {
                string path = GetGameObjectNameFromTreeViewItem(clickedItem);
                string rootName = Prefab.name;
                if (path.Equals(rootName))
                {
                    target = Prefab.gameObject;
                }
                else
                {
                    //第二节点
                    target = Prefab.transform.Find(path).gameObject;
                }

                Component[] components = target.GetComponents<Component>();
                List<Component> legalComps = FiterComponent(components);
                m_nowClikComponent =legalComps.Count>0?legalComps[0]:null;
                m_nowComponentsList = legalComps;
                m_nowClikNodeObj = target;
            }
        }

        public List<Component> FiterComponent(Component[] components)
        {
            List<Component> coms = new List<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if (components[i] is Image ||
                    components[i] is Button ||
                    components[i] is Text ||
                    components[i] is Slider ||
                    components[i] is Canvas ||
                    components[i] is ScrollRect ||
                    components[i] is Toggle
                   )
                {
                    coms.Add(components[i]);
                }
            }

            return coms;
        }

        public Component[] ReturnSingleClickedItem(int id)
        {
            GameObject target = null;
            base.SingleClickedItem(id);

            TreeViewItem clickedItem = FindItem(id, rootItem);

            if (clickedItem != null)
            {
                string path = GetGameObjectNameFromTreeViewItem(clickedItem);
                string rootName = Prefab.name;
                if (path.Equals(rootName))
                {
                    target = Prefab.gameObject;
                }
                else
                {
                    //第二节点
                    target = Prefab.transform.Find(path).gameObject;
                }

                Component[] components = target.GetComponents<Component>();

                return components;
            }

            return null;
        }
        public GameObject ReturnSingleClickeGameObject(int id)
        {
            GameObject target = null;
            base.SingleClickedItem(id);

            TreeViewItem clickedItem = FindItem(id, rootItem);

            if (clickedItem != null)
            {
                string path = GetGameObjectNameFromTreeViewItem(clickedItem);
                string rootName = Prefab.name;
                if (path.Equals(rootName))
                {
                    target = Prefab.gameObject;
                }
                else
                {
                    //第二节点
                    target = Prefab.transform.Find(path).gameObject;
                }
                return target;
            }

            return null;
        }
        /// <summary>
        /// 获取点击节点名称根据id
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetGameObjectNameFromTreeViewItem(TreeViewItem item)
        {
            if (item.depth == 0) // Top-level item
            {
                return item.displayName; // This is the prefab's root GameObject
            }

            // Traverse up the tree to find the root GameObject's children
            List<string> path = new List<string>();
            TreeViewItem currentItem = item;
            while (currentItem.depth > 0)
            {
                path.Insert(0, currentItem.displayName);
                currentItem = currentItem.parent;
            }

            // Now you have the path from the root to the clicked GameObject
            // Build the full GameObject name using the path
            string gameObjectName = string.Join("/", path);
            return gameObjectName;
        }
    }
}