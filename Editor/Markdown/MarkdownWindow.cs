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
        /// 打开窗口
        /// </summary>
        /// <param name="text">Markdown文本</param>
        public static void OpenWindow(TextAsset textAsset)
        {
            MarkdownWindow window = GetWindow<MarkdownWindow>();
            window.titleContent.text = "Markdown Window";
            window.Show();

            GUISkin skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/HTModuleManager/Editor/Markdown/MarkdownSkin.guiskin");
            window._viewer = new MarkdownViewer(skin, AssetDatabase.GetAssetPath(textAsset), textAsset.text);
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