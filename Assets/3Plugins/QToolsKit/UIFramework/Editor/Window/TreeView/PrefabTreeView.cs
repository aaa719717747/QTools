using System.Collections.Generic;
using System.Linq;
using _3Plugins.QToolsKit.UIFramework.Editor.Utils;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.Data;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView
{
    public class PrefabTreeView : UnityEditor.IMGUI.Controls.TreeView
    {
        public GameObject Prefab { get; set; }

        public PrefabTreeView(TreeViewState treeViewState, GameObject prefab)
            : base(treeViewState)
        {
            Prefab = prefab;
            Reload();
        }

        protected override TreeViewItem BuildRoot()
        {
            var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
            List<TreeViewItem> allItems = new List<TreeViewItem> { root };

            if (Prefab != null)
            {
                var topLevelNode = new TreeViewItem { id = 1, depth = 0, displayName = Prefab.name, parent = root };
                root.children = new List<TreeViewItem> { topLevelNode };
                allItems.Add(topLevelNode);

                int id = 2;
                TraverseTransform(Prefab.transform, 1, ref allItems, ref id, topLevelNode);
            }
            else
            {
                allItems.Add(new TreeViewItem { id = 1, depth = 0, displayName = "No Prefab Selected" });
            }

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
        }

        /// <summary>
        /// 点击子叶节点事件
        /// </summary>
        /// <param name="id"></param>
        protected override void SingleClickedItem(int id)
        {
            Debug.Log(id);
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

                FormWindowData.m_nowClikComponent = components[0];
                FormWindowData.m_UpdateNodeComponentsList(components.ToList());
            }
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