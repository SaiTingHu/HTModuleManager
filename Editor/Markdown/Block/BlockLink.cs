using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown文本超链接【[文本超链接](url)】
    /// </summary>
    internal class BlockLink : MarkdownBlock
    {
        private Object _object;
        private GUIContent _gc;

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; private set; }
        /// <summary>
        /// 是否为资产链接
        /// </summary>
        public bool IsAssetsUrl { get; private set; }

        public BlockLink(string text, string url) : base(text)
        {
            _gc = new GUIContent(text, url);

            Url = url;
            IsAssetsUrl = Url.StartsWith("Assets/");

            if (IsAssetsUrl)
            {
                _object = AssetDatabase.LoadAssetAtPath<Object>(Url);
            }
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            Color color = GUI.contentColor;
            GUI.contentColor = (IsAssetsUrl && _object == null) ? Container.Document.ErrorColor : Container.Document.LinkColor;
            if (GUILayout.Button(_gc, "Label"))
            {
                if (IsAssetsUrl)
                {
                    if (_object != null) EditorGUIUtility.PingObject(_object);
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