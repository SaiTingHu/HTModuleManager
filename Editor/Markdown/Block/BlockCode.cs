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
            Color color = GUI.contentColor;
            GUI.contentColor = Color.yellow;
            GUILayout.Box(Text);
            GUI.contentColor = color;
        }
    }
}