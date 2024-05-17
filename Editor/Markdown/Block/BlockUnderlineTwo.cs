using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown加粗文本【__加粗文本__】
    /// </summary>
    internal class BlockUnderlineTwo : MarkdownBlock
    {
        public BlockUnderlineTwo(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            skin.label.fontStyle = FontStyle.Bold;
            GUILayout.Label(Text);
            skin.label.fontStyle = FontStyle.Normal;
        }
    }
}