using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown强调文本【_强调文本_】
    /// </summary>
    internal class BlockUnderline : MarkdownBlock
    {
        public BlockUnderline(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            FontStyle fontStyle = skin.label.fontStyle;
            skin.label.fontStyle = FontStyle.Italic;
            GUILayout.Label(Text);
            skin.label.fontStyle = fontStyle;
        }
    }
}
