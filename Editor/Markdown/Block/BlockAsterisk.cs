using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown强调文本【*强调文本*】
    /// </summary>
    internal class BlockAsterisk : MarkdownBlock
    {
        public BlockAsterisk(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            skin.label.fontStyle = FontStyle.Italic;
            GUILayout.Label(Text);
            skin.label.fontStyle = FontStyle.Normal;
        }
    }
}