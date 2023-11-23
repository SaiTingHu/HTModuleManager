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
            window.position = new Rect(moduleManager.position.center - new Vector2(150, 45), new Vector2(300, 90));
            window.minSize = new Vector2(300, 90);
            window.maxSize = new Vector2(300, 90);
            window.Show();
        }

        private const string _tortoiseGitVersion = "2.14.0.0";
        private ModuleManagerWindow _moduleManager;
        private string _tortoiseGitPath;
        private string _autoInclusionPath;
        private GUIContent _versionGC;

        private void OnEnable()
        {
            _tortoiseGitPath = EditorPrefs.GetString(Utility.TortoiseGitPath, "");
            _autoInclusionPath = EditorPrefs.GetString(Utility.AutoInclusionPath, "");
            _versionGC = new GUIContent();
            _versionGC.text = "Version:";
            _versionGC.tooltip = "Recommended version";
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(_versionGC, GUILayout.Width(100));
            GUILayout.Label(_tortoiseGitVersion, EditorStyles.linkLabel);
            GUILayout.FlexibleSpace();
            GUI.enabled = !string.IsNullOrEmpty(_tortoiseGitPath);
            if (GUILayout.Button("Local version"))
            {
                TortoiseGitHelper.About();
            }
            GUI.enabled = true;
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Path:", GUILayout.Width(100));
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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Auto Inclusion:", GUILayout.Width(100));
            _autoInclusionPath = EditorGUILayout.TextField(_autoInclusionPath);
            if (GUILayout.Button("Browse", EditorStyles.miniButton, GUILayout.Width(60)))
            {
                string path = EditorUtility.OpenFilePanel("Select Auto Inclusion Path", Application.dataPath, "autoim");
                if (path.Length != 0 && path.StartsWith(Application.dataPath))
                {
                    _autoInclusionPath = path.Replace(Application.dataPath, "");
                    GUI.FocusControl(null);
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sure", "ButtonLeft"))
            {
                EditorPrefs.SetString(Utility.TortoiseGitPath, _tortoiseGitPath);
                EditorPrefs.SetString(Utility.AutoInclusionPath, _autoInclusionPath);
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