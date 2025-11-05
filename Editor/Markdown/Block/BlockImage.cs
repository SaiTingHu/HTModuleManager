using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown图像【![图像](path)】
    /// </summary>
    internal class BlockImage : MarkdownBlock
    {
        private Texture2D _texture;
        private bool _isLoad;
        private bool _isLoadFail;

        /// <summary>
        /// 图像路径
        /// </summary>
        public string Path { get; private set; }

        public BlockImage(string text, string path) : base(text)
        {
            _isLoad = false;
            _isLoadFail = false;

            Path = path;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            LoadTexture();

            if (_isLoadFail && Container.Document.IsHideLoadFailImage)
                return;

            float width = Mathf.Min(EditorGUIUtility.currentViewWidth - 10, _texture.width);
            float height = width * ((float)_texture.height / _texture.width);
            GUILayout.Label(_texture, GUILayout.Width(width), GUILayout.Height(height));
        }

        /// <summary>
        /// 加载图像
        /// </summary>
        private void LoadTexture()
        {
            if (!_isLoad)
            {
                if (Path.StartsWith("https://"))
                {
                    _isLoadFail = true;
                    _texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Markdown/Texture/LoadFail.png");
                }
                else
                {
                    string assetPath = Container.Document.AssetPath;
                    string fullPath = assetPath.Substring(0, assetPath.LastIndexOf('/') + 1) + Path;
                    _texture = AssetDatabase.LoadAssetAtPath<Texture2D>(fullPath);
                    if (_texture == null)
                    {
                        _isLoadFail = true;
                        _texture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Markdown/Texture/LoadFail.png");
                    }
                }
                _isLoad = true;
            }
        }
    }
}