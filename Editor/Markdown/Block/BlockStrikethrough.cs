using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown删除线【~~删除线~~】
    /// </summary>
    internal class BlockStrikethrough : MarkdownBlock
    {
        public BlockStrikethrough(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            Color color = GUI.color;
            GUI.color = Color.gray;
            GUILayout.Label(Text);
            GUI.color = color;

            GUI.skin = null;

            Rect rect = GUILayoutUtility.GetLastRect();
            rect.y += (rect.height * 0.5f - 1);
            rect.height = 1.5f;

            color = GUI.color;
            GUI.color = Color.yellow;
            GUI.Box(rect, "", "WhiteBackground");
            GUI.color = color;

            GUI.skin = skin;
        }
    }
}