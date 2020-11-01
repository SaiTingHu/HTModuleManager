using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    internal sealed class ModuleManagerWindow : EditorWindow
    {
        [MenuItem("HT/Module Manager", priority = 2000)]
        private static void OpenWindow()
        {
            ModuleManagerWindow window = GetWindow<ModuleManagerWindow>();
            window.titleContent.text = "Module Manager";
            window.Show();
        }
        
        private ModulesLibrary _modulesLibrary;
        private Module _currentModule;
        private ModuleType _showModuleType = ModuleType.InProject;
        private bool _isCreateModule;
        private string _createModuleLocalPath;
        private string _createModuleRemotePath;
        private Texture2D _github;
        private GUIContent _moduleGC;
        private GUIContent _downloadedGC;
        private GUIContent _noDownloadedGC;
        private GUIContent _githubGC;
        private GUIContent _networkGC;
        private Vector2 _scroll;
        
        private void OnEnable()
        {
            if (_modulesLibrary == null)
            {
                _modulesLibrary = new ModulesLibrary(GetNativeModulesDefine());
            }
            else
            {
                _modulesLibrary.RefreshState();
            }
            _currentModule = null;
            _isCreateModule = false;
            _createModuleLocalPath = "";
            _createModuleRemotePath = "";
            _github = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Texture/Github.png");
            _moduleGC = new GUIContent();
            _moduleGC.image = EditorGUIUtility.IconContent("Folder Icon").image;
            _downloadedGC = new GUIContent();
            _downloadedGC.image = EditorGUIUtility.IconContent("Collab").image;
            _noDownloadedGC = new GUIContent();
            _noDownloadedGC.image = EditorGUIUtility.IconContent("CollabConflict").image;
            _githubGC = new GUIContent();
            _githubGC.image = _github;
            _networkGC = new GUIContent();
            _networkGC.image = EditorGUIUtility.IconContent("BuildSettings.Web.Small").image;
        }
        private void OnGUI()
        {
            OnTitleGUI();

            GUILayout.BeginHorizontal();

            OnModuleListGUI();

            GUILayout.Space(5);

            GUILayout.Box("", "DopesheetBackground", GUILayout.Width(5), GUILayout.ExpandHeight(true));

            GUILayout.Space(5);

            OnModuleGUI();

            GUILayout.EndHorizontal();
        }
        private void OnDestroy()
        {
            if (_modulesLibrary != null)
            {
                _modulesLibrary.Dispose();
                _modulesLibrary = null;
            }
        }
        /// <summary>
        /// 标题GUI
        /// </summary>
        private void OnTitleGUI()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button(_showModuleType.ToString(), EditorStyles.toolbarDropDown))
            {
                GenericMenu gm = new GenericMenu();
                gm.AddItem(new GUIContent("All Module"), _showModuleType == ModuleType.AllModule, () =>
                {
                    _showModuleType = ModuleType.AllModule;
                    _currentModule = null;
                });
                gm.AddItem(new GUIContent("In Project"), _showModuleType == ModuleType.InProject, () =>
                {
                    _showModuleType = ModuleType.InProject;
                    _currentModule = null;
                });
                gm.ShowAsContext();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Credentials", EditorStyles.toolbarButton))
            {
                CredentialsProviderWindow.OpenWindow(this, _modulesLibrary.SetCredentials);
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// 所有模块列表GUI
        /// </summary>
        private void OnModuleListGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(300));

            _scroll = GUILayout.BeginScrollView(_scroll);

            for (int i = 0; i < _modulesLibrary.Modules.Count; i++)
            {
                if (ModuleIsShow(_modulesLibrary.Modules[i]))
                {
                    if (_currentModule == _modulesLibrary.Modules[i]) GUILayout.BeginHorizontal("InsertionMarker");
                    else GUILayout.BeginHorizontal();

                    GUI.color = _modulesLibrary.Modules[i].IsLocalExist ? Color.white : Color.gray;
                    _moduleGC.text = _modulesLibrary.Modules[i].Name;
                    if (GUILayout.Button(_moduleGC, EditorStyles.label, GUILayout.Height(24)))
                    {
                        _currentModule = _modulesLibrary.Modules[i];
                        GUI.FocusControl(null);
                    }
                    GUI.color = Color.white;

                    GUILayout.FlexibleSpace();

                    GUILayout.Label(_modulesLibrary.Modules[i].IsLocalExist ? _downloadedGC : _noDownloadedGC);

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();

            if (_isCreateModule)
            {
                GUILayout.BeginVertical("Box");

                GUILayout.BeginHorizontal();
                GUILayout.Label("Local", GUILayout.Width(60));
                _createModuleLocalPath = EditorGUILayout.TextField(_createModuleLocalPath);
                if (GUILayout.Button("Browse", "MiniButton", GUILayout.Width(60)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Local Path", Application.dataPath, "");
                    if (path != "")
                    {
                        _createModuleLocalPath = path;
                        GUI.FocusControl(null);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Remote", GUILayout.Width(60));
                _createModuleRemotePath = EditorGUILayout.TextField(_createModuleRemotePath);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create", "ButtonLeft"))
                {
                    _modulesLibrary.CreateNullModule(_createModuleLocalPath, _createModuleRemotePath);
                    _isCreateModule = false;
                }
                if (GUILayout.Button("Cancel", "ButtonRight"))
                {
                    _isCreateModule = false;
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Module", "ButtonLeft"))
            {
                _isCreateModule = true;
            }
            if (GUILayout.Button("Open Module", "ButtonMid"))
            {
                string path = EditorUtility.OpenFolderPanel("Open Module", Application.dataPath, "");
                if (path != "")
                {
                    _modulesLibrary.OpenModule(path);
                }
            }
            GUI.enabled = _currentModule != null;
            if (GUILayout.Button("Remove Module", "ButtonMid"))
            {
                _modulesLibrary.RemoveModule(_currentModule);
                _currentModule = null;
            }
            GUI.enabled = true;
            if (GUILayout.Button("Pull All", "ButtonRight"))
            {
                if (EditorUtility.DisplayDialog("Pull All", "Are you sure you want to pull all modules?", "Yes", "No"))
                {
                    _modulesLibrary.PullAll();
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.EndVertical();
        }
        /// <summary>
        /// 模块GUI
        /// </summary>
        private void OnModuleGUI()
        {
            if (_currentModule == null)
                return;

            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(_currentModule.Name, "LargeBoldLabel");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUI.enabled = _currentModule.IsLocalExist;
            if (GUILayout.Button("Local", "ButtonLeft"))
            {
                ProcessStartInfo psi = new ProcessStartInfo(_currentModule.Path);
                Process.Start(psi);
            }
            GUI.enabled = _currentModule.IsRemoteExist;
            if (GUILayout.Button("Remote", "ButtonRight"))
            {
                Application.OpenURL(_currentModule.RemotePath);
            }
            GUI.enabled = !_currentModule.IsLocalExist && _currentModule.IsRemoteExist;
            if (GUILayout.Button("Clone", "ButtonLeft"))
            {
                _modulesLibrary.Clone(_currentModule);
                AssetDatabase.Refresh();
            }
            GUI.enabled = _currentModule.IsLocalExist && _currentModule.IsRemoteExist;
            if (GUILayout.Button("Pull", "ButtonRight"))
            {
                _modulesLibrary.Pull(_currentModule);
                AssetDatabase.Refresh();
            }
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Local Path", GUILayout.Width(80), GUILayout.Height(20));
            GUILayout.Label(_currentModule.Path, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Remote Path", GUILayout.Width(80), GUILayout.Height(20));
            _githubGC.text = _networkGC.text = _currentModule.RemotePath;
            GUILayout.Label(_currentModule.IsGithub ? _githubGC : _networkGC, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (!_currentModule.IsLocalExist)
            {
                EditorGUILayout.HelpBox("There is no this repository locally! please clone to local first.", MessageType.Warning);
            }
            
            GUILayout.EndVertical();
        }
        
        /// <summary>
        /// 获取原生模块的定义
        /// </summary>
        private string[] GetNativeModulesDefine()
        {
            string[] modules = File.ReadAllLines(Application.dataPath + "/HTModuleManager/Editor/Manager/NativeModules.txt");
            for (int i = 0; i < modules.Length; i++)
            {
                modules[i] = Application.dataPath + "/" + modules[i];
            }
            return modules;
        }
        /// <summary>
        /// 模块是否显示
        /// </summary>
        /// <param name="module">模块</param>
        /// <returns>是否显示</returns>
        private bool ModuleIsShow(Module module)
        {
            return _showModuleType == ModuleType.AllModule ? true : module.IsLocalExist;
        }
        
        /// <summary>
        /// 模块类型
        /// </summary>
        public enum ModuleType
        {
            /// <summary>
            /// 所有模块
            /// </summary>
            AllModule,
            /// <summary>
            /// 已存在项目中的模块
            /// </summary>
            InProject
        }
    }
}