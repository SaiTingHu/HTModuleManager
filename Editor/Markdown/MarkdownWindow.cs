using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// 通用Markdown查看窗口
    /// </summary>
    public sealed class MarkdownWindow : EditorWindow
    {
        /// <summary>
        /// 打开窗口（查看文本文件）
        /// </summary>
        /// <param name="text">Markdown文本</param>
        public static void OpenWindowOfFile(TextAsset textAsset)
        {
            MarkdownWindow window = GetWindow<MarkdownWindow>();
            window.titleContent.text = "Markdown Window";
            window.Show();

            GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/HTModuleManager/Editor/Markdown/MarkdownSkin.guiskin");
            window._viewer = new MarkdownViewer(skin, AssetDatabase.GetAssetPath(textAsset), textAsset.text);
        }
        /// <summary>
        /// 打开窗口（查看文本内容）
        /// </summary>
        /// <param name="content">Markdown文本内容</param>
        public static void OpenWindowOfContent(string content)
        {
            MarkdownWindow window = GetWindow<MarkdownWindow>();
            window.titleContent.text = "Markdown Window";
            window.Show();

            GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/HTModuleManager/Editor/Markdown/MarkdownSkin.guiskin");
            window._viewer = new MarkdownViewer(skin, Application.dataPath, content);
        }

        private MarkdownViewer _viewer;

        private void OnGUI()
        {
            if (_viewer != null)
            {
                _viewer.Draw();
            }
        }
    }
}