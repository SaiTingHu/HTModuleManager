using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown引用对象【{Object}(url)】
    /// </summary>
    internal class BlockObject : MarkdownBlock
    {
        private Object _object;
        private Texture _icon;
        private GUIContent _gc;

        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; private set; }

        public BlockObject(string text, string url) : base(text)
        {
            _object = AssetDatabase.LoadAssetAtPath<Object>(url);
            _icon = _object != null ? EditorGUIUtility.ObjectContent(_object, typeof(Object)).image : null;
            _gc = _object != null ? new GUIContent(_object.name, url) : GUIContent.none;

            Url = url;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            if (_object != null && _icon != null)
            {
                GUILayout.Label(_icon, GUILayout.Width(16), GUILayout.Height(26));
                Color color = GUI.contentColor;
                GUI.contentColor = Container.Document.ObjectColor;
                if (GUILayout.Button(_gc, "Label"))
                {
                    EditorGUIUtility.PingObject(_object);
                }
                Rect rect = GUILayoutUtility.GetLastRect();
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
                GUI.contentColor = color;
            }
            else
            {
                GUILayout.Label("Missing Object");
            }
        }
    }
}