using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown代码【`代码`】
    /// </summary>
    internal class BlockCode : MarkdownBlock
    {
        public BlockCode(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            Color cColor = GUI.contentColor;
            Color bgColor = GUI.backgroundColor;
            GUI.contentColor = Color.yellow;
            GUI.backgroundColor = Color.black;
            GUILayout.Box(Text);
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (GUI.Button(rect, "", "Label"))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("Copy"), false, () =>
                {
                    GUIUtility.systemCopyBuffer = Text;
                });
                gm.AddDisabledItem(new GUIContent("Paste"), false);
                gm.ShowAsContext();
            }
            GUI.contentColor = cColor;
            GUI.backgroundColor = bgColor;
        }
    }
}