using System;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// 凭据管理界面
    /// </summary>
    internal sealed class CredentialsProviderWindow : EditorWindow
    {
        public static void OpenWindow(ModuleManagerWindow moduleManager, Action<string, string> setCredentialsAction)
        {
            CredentialsProviderWindow window = GetWindow<CredentialsProviderWindow>();
            window.titleContent.text = "Credentials Provider";
            window._moduleManager = moduleManager;
            window._setCredentialsAction = setCredentialsAction;
            window.position = new Rect(moduleManager.position.center - new Vector2(125, 0), new Vector2(250, 50));
            window.minSize = new Vector2(250, 50);
            window.maxSize = new Vector2(250, 50);
            window.Show();
        }

        private ModuleManagerWindow _moduleManager;
        private Action<string, string> _setCredentialsAction;
        private string _userName;
        private string _email;

        private void OnEnable()
        {
            _userName = EditorPrefs.GetString(Utility.UserNameKey, "");
            _email = EditorPrefs.GetString(Utility.EmailKey, "");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("UserName:", GUILayout.Width(80));
            _userName = EditorGUILayout.TextField(_userName);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Email:", GUILayout.Width(80));
            _email = EditorGUILayout.TextField(_email);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sure", "ButtonLeft"))
            {
                _setCredentialsAction?.Invoke(_userName, _email);
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