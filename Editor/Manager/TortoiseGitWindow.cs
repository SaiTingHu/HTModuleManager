using System;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// TortoiseGit配置窗口
    /// </summary>
    internal sealed class TortoiseGitWindow : EditorWindow
    {
        public static void OpenWindow(ModuleManagerWindow moduleManager)
        {
            TortoiseGitWindow window = GetWindow<TortoiseGitWindow>();
            window.titleContent.text = "Tortoise Git";
            window._moduleManager = moduleManager;
            window.position = new Rect(moduleManager.position.center - new Vector2(125, 35), new Vector2(250, 70));
            window.minSize = new Vector2(250, 70);
            window.maxSize = new Vector2(250, 70);
            window.Show();
        }

        private const string _tortoiseGitVersion = "2.13.0.1";
        private ModuleManagerWindow _moduleManager;
        private string _tortoiseGitPath;

        private void OnEnable()
        {
            _tortoiseGitPath = EditorPrefs.GetString(Utility.TortoiseGitPath, "");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Version:", GUILayout.Width(60));
            GUILayout.Label(_tortoiseGitVersion, EditorStyles.linkLabel);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Path:", GUILayout.Width(60));
            _tortoiseGitPath = EditorGUILayout.TextField(_tortoiseGitPath);
            if (GUILayout.Button("Browse", EditorStyles.miniButton, GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFilePanel("Select Tortoise Git Path", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "exe");
                if (path.Length != 0)
                {
                    _tortoiseGitPath = path;
                    GUI.FocusControl(null);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sure", "ButtonLeft"))
            {
                EditorPrefs.SetString(Utility.TortoiseGitPath, _tortoiseGitPath);
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