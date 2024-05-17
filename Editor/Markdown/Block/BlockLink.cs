using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown文本超链接【[文本超链接](url)】
    /// </summary>
    internal class BlockLink : MarkdownBlock
    {
        private readonly Color _linkColor = new Color(0.298f, 0.494f, 1, 1);

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; private set; }

        public BlockLink(string text, string url) : base(text)
        {
            Url = url;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            GUI.color = _linkColor;
            if (GUILayout.Button(Text, "Label"))
            {
                Application.OpenURL(Url);
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.color = Color.white;
        }
    }
}