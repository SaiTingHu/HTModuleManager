using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown执行菜单【{Menu}(MenuPath)】
    /// </summary>
    internal class BlockMenu : MarkdownBlock
    {
        private Texture _icon;
        private GUIContent _gc;

        /// <summary>
        /// 菜单路径
        /// </summary>
        public string Path { get; private set; }

        public BlockMenu(string text, string path) : base(text)
        {
            _icon = EditorGUIUtility.IconContent("UnityEditor.ConsoleWindow").image;
            _gc = new GUIContent(path.Substring(path.LastIndexOf('/') + 1), path);

            Path = path;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            GUILayout.Label(_icon, GUILayout.Width(skin.label.fontSize + 2), GUILayout.Height(skin.label.fontSize + 12));
            Color color = GUI.contentColor;
            GUI.contentColor = Container.Document.MenuColor;
            if (GUILayout.Button(_gc, "Label"))
            {
                EditorApplication.ExecuteMenuItem(Path);
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.contentColor = color;
        }
    }
}