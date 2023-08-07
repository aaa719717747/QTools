using System.Collections.Generic;
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
            Texture2D icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/3Plugins/LanguageLocalizationHelper/Editor/Textures/text.jpg");

            if (icon != null)
            {
                // 计算图标的位置
                Rect iconRect = new Rect(args.rowRect);
                iconRect.x+=250; // 10是图标和行末尾的间距
                iconRect.width = 15;//icon.width;
                iconRect.height = 15;//icon.height;
                // 绘制图标
                GUI.DrawTexture(iconRect, icon);
            }
        }

        protected override void SingleClickedItem(int id)
        {
            base.SingleClickedItem(id);
            Debug.Log($"clik:{id}");
        }
    }
}