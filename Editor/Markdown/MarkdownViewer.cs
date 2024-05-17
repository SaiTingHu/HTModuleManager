using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown查看器
    /// </summary>
    public sealed class MarkdownViewer
    {
        private GUISkin _skin;
        private string _assetPath;
        private string _content;
        private MarkdownParser _parser;
        private MarkdownDocument _document;

        /// <summary>
        /// Markdown查看器
        /// </summary>
        /// <param name="skin">皮肤</param>
        /// <param name="assetPath">资源文件路径</param>
        /// <param name="content">文件内容</param>
        /// <param name="isHideLoadFailImage">是否隐藏加载失败的图像</param>
        public MarkdownViewer(GUISkin skin, string assetPath, string content, bool isHideLoadFailImage = false)
        {
            _skin = skin;
            _assetPath = assetPath;
            _content = content;
            _parser = new MarkdownParser();
            _document = _parser.Parse(_assetPath, _content, isHideLoadFailImage);
        }

        /// <summary>
        /// 绘制文档
        /// </summary>
        public void Draw()
        {
            if (_document != null)
            {
                _document.Draw(_skin);
            }
            else
            {
                GUILayout.Label(_content);
            }
        }
    }
}