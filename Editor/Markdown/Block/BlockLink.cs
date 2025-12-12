using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown文本超链接【[文本超链接](url)】
    /// </summary>
    internal class BlockLink : MarkdownBlock
    {
        private GUIContent _gc;

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; private set; }

        public BlockLink(string text, string url) : base(text)
        {
            _gc = new GUIContent(text, url);

            Url = url;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            Color color = GUI.contentColor;
            GUI.contentColor = Container.Document.LinkColor;
            if (GUILayout.Button(_gc, "Label"))
            {
                if (Url.StartsWith("Assets/"))
                {
                    Object obj = AssetDatabase.LoadAssetAtPath<Object>(Url);
                    if (obj != null)
                    {
                        EditorGUIUtility.PingObject(obj);
                    }
                }
                else
                {
                    Application.OpenURL(Url);
                }
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.contentColor = color;
        }
    }
}