using System.Collections.Generic;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown文档
    /// </summary>
    internal sealed class MarkdownDocument
    {
        private List<MarkdownContainer> _containers;
        private Vector2 _scroll;

        /// <summary>
        /// 文档资源路径
        /// </summary>
        public string AssetPath { get; private set; }
        /// <summary>
        /// 是否隐藏加载失败的图像
        /// </summary>
        public bool IsHideLoadFailImage { get; private set; }

        public MarkdownDocument(string assetPath, bool isHideLoadFailImage)
        {
            _containers = new List<MarkdownContainer>();
            _scroll = Vector2.zero;

            AssetPath = assetPath;
            IsHideLoadFailImage = isHideLoadFailImage;
        }

        /// <summary>
        /// 添加一个容器对象到文档中
        /// </summary>
        /// <param name="container">容器对象</param>
        public void AddContainer(MarkdownContainer container)
        {
            if (container == null)
                return;

            container.Document = this;
            _containers.Add(container);
        }
        /// <summary>
        /// 绘制文档
        /// </summary>
        public void Draw(GUISkin skin)
        {
            GUI.skin = skin;

            GUILayout.BeginVertical();
            _scroll = GUILayout.BeginScrollView(_scroll);
            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].Draw(skin);
            }
            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}