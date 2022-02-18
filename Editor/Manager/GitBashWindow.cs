using System;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// GitBash管理界面
    /// </summary>
    internal sealed class GitBashWindow : EditorWindow
    {
        public static void OpenWindow(ModuleManagerWindow moduleManager)
        {
            GitBashWindow window = GetWindow<GitBashWindow>();
            window.titleContent.text = "Git Bash";
            window._moduleManager = moduleManager;
            window.position = new Rect(moduleManager.position.center - new Vector2(125, 0), new Vector2(250, 50));
            window.minSize = new Vector2(250, 50);
            window.maxSize = new Vector2(250, 50);
            window.Show();
        }

        private ModuleManagerWindow _moduleManager;
        private string _gitBashPath;

        private void OnEnable()
        {
            _gitBashPath = EditorPrefs.GetString(Utility.GitBashPath, "");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Path:", GUILayout.Width(60));
            _gitBashPath = EditorGUILayout.TextField(_gitBashPath);
            if (GUILayout.Button("Browse", EditorStyles.miniButton, GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFilePanel("Select Git Bash Path", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "exe");
                if (path.Length != 0)
                {
                    _gitBashPath = path;
                    GUI.FocusControl(null);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sure", "ButtonLeft"))
            {
                EditorPrefs.SetString(Utility.GitBashPath, _gitBashPath);
                Close();
            }
            if (GUILayout.Button("Cancel", "ButtonRight"))
            {
                Close();
            }
            GUILayout.EndHorizontal();
        }

        private void Update()
        {
            if (_moduleManager == null)
            {
                Close();
            }
        }
    }
}