using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown图像超链接【[![图像超链接](path)](url)】
    /// </summary>
    internal class BlockImageLink : MarkdownBlock
    {
        private Texture2D _texture;
        private bool _isLoad;
        private bool _isLoadFail;

        /// <summary>
        /// 图像路径
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string Url { get; private set; }

        public BlockImageLink(string text, string path, string url) : base(text)
        {
            _isLoad = false;
            _isLoadFail = false;

            Path = path;
            Url = url;
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            LoadTexture();

            if (_isLoadFail && Container.Document.IsHideLoadFailImage)
                return;

            GUILayout.Label(_texture);
            Rect rect = GUILayoutUtility.GetLastRect();
            EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
            if (GUI.Button(rect, "", "Label"))
            {
                Application.OpenURL(Url);
            }
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