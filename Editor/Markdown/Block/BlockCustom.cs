using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown自定义操作【{Custom}(args)】
    /// </summary>
    internal class BlockCustom : MarkdownBlock
    {
        private GUIContent _gc;

        /// <summary>
        /// 参数
        /// </summary>
        public string Args { get; private set; }

        public BlockCustom(string text, string args) : base(text)
        {
            _gc = new GUIContent(args, "Custom：" + args);

            Args = args;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            Color color = GUI.contentColor;
            GUI.contentColor = Container.Document.CustomColor;
            if (GUILayout.Button(_gc, "Label"))
            {
                Container.Document.DoCustomAction(Args);
            }
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            GUI.contentColor = color;
        }
    }
}