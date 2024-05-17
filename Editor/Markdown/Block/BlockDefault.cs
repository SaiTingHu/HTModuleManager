using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown普通文本
    /// </summary>
    internal class BlockDefault : MarkdownBlock
    {
        public BlockDefault(string text) : base(text)
        {

        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            GUILayout.Label(Text);
        }
    }
}