using System.Collections.Generic;
using _3Plugins.QToolsKit.UIFramework.Editor.Window.TreeView;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class PrefabTreeViewWindow : EditorWindow
{
    // SerializeField is used to ensure the view state is written to the window 
    // layout file. This means that the state survives restarting Unity as long as the window
    // is not closed. If the attribute is omitted then the state is still serialized/deserialized.
    [SerializeField] TreeViewState m_TreeViewState;

    //The TreeView is not serializable, so it should be reconstructed from the tree data.
    PrefabTreeView m_SimpleTreeView;
    
    private Vector2 leftScrollPosition;
    private Vector2 treeviewScrollPosition;
    private Vector2 rightScrollPosition;
    private string searchQuery = "";
    void OnEnable()
    {
        // Check whether there is already a serialized view state (state 
        // that survived assembly reloading)
        if (m_TreeViewState == null)
            m_TreeViewState = new TreeViewState();

        GameObject preab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/UI/RegisterForm.prefab");
        m_SimpleTreeView = new PrefabTreeView(m_TreeViewState, preab);
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        // 左侧窗口
        EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox],GUILayout.Width(position.width / 3));
        leftScrollPosition = EditorGUILayout.BeginScrollView(leftScrollPosition);
        if (GUILayout.Button("Button 1"))
        {
            Debug.Log("Button 1 clicked");
        }
        
        EditorGUILayout.LabelField("Search Field Example", EditorStyles.boldLabel);

        // 绘制搜索框
        // searchQuery = EditorGUILayout.ToolbarSearchField(searchQuery);

        // 显示搜索结果
        EditorGUILayout.LabelField("Search query: " + searchQuery);
        
        
        EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox],GUILayout.Width(position.width / 3));
        treeviewScrollPosition= EditorGUILayout.BeginScrollView(leftScrollPosition);
        // 在这里渲染目录树
        if (m_SimpleTreeView != null)
        {
            m_SimpleTreeView.OnGUI(new Rect(0, 0, position.width / 3, position.height));
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
      

        
        
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        // 右侧窗口
        EditorGUILayout.BeginVertical(this[LayoutStyle.GroupBox],GUILayout.Width(position.width-(position.width / 3)));
        rightScrollPosition = EditorGUILayout.BeginScrollView(rightScrollPosition);
        // 在这里渲染按钮和文本区域
        EditorGUILayout.LabelField("Render your buttons and text areas here");
        if (GUILayout.Button("Button 1"))
        {
            Debug.Log("Button 1 clicked");
        }
        if (GUILayout.Button("Button 2"))
        {
            Debug.Log("Button 2 clicked");
        }
        EditorGUILayout.TextArea("This is a text area");
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
    // Add menu named "My Window" to the Window menu
    [MenuItem("TreeView Examples/Simple Tree Window")]
    static void ShowWindow()
    {
        // Get existing open window or if none, make a new one:
        var window = GetWindow<PrefabTreeViewWindow>();
        window.titleContent = new GUIContent("My Window");
        window.Show();
    }


    /// <summary>
    /// 模块样式索引
    /// </summary>
    /// <param name="ls"></param>
    string this[LayoutStyle ls]
    {
        get
        {
            string str = "";
            switch (ls)
            {
                case LayoutStyle.Title:
                    str = "MeTransOffRight";
                    break;
                case LayoutStyle.AddBtn:
                    str = "CN CountBadge";
                    break;
                case LayoutStyle.GroupBox:
                    str = "GroupBox";
                    break;
                case LayoutStyle.HelpBox:
                    str = "HelpBox";
                    break;
                case LayoutStyle.FoldOut:
                    str = "Foldout";
                    break;
                case LayoutStyle.Toggle:
                    str = "BoldToggle";
                    break;
            }

            return str;
        }
    }

    public enum LayoutStyle
    {
        Title,
        AddBtn,
        GroupBox,
        HelpBox,
        FoldOut,
        Toggle,
    }
}