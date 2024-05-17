using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown语法块
    /// </summary>
    internal abstract class MarkdownBlock
    {
        /// <summary>
        /// 所属的容器
        /// </summary>
        public MarkdownContainer Container { get; set; }
        /// <summary>
        /// 文本内容
        /// </summary>
        public string Text { get; private set; }

        public MarkdownBlock(string text)
        {
            Text = text;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public abstract void Draw(GUISkin skin);
    }
}