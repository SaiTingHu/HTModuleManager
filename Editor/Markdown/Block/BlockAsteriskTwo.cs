using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown加粗文本【**加粗文本**】
    /// </summary>
    internal class BlockAsteriskTwo : MarkdownBlock
    {
        public BlockAsteriskTwo(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            FontStyle fontStyle = skin.label.fontStyle;
            skin.label.fontStyle = FontStyle.Bold;
            GUILayout.Label(Text);
            skin.label.fontStyle = fontStyle;
        }
    }
}