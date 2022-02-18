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
        private Module _currentEditModule;
        private TextAsset _currentModuleReadme;
        private DefaultAsset _currentModuleLICENSE;
        private ModuleType _showModuleType = ModuleType.InProject;
        private bool _isCreateModule;
        private bool _isEditModule;
        private string _inputModuleLocalPath;
        private string _inputModuleRemotePath;
        private Texture2D _gitBash;
        private Texture2D _github;
        private Texture2D _gitee;
        private GUIContent _moduleGC;
        private GUIContent _downloadedGC;
        private GUIContent _noDownloadedGC;
        private GUIContent _gitBashGC;
        private GUIContent _githubGC;
        private GUIContent _giteeGC;
        private GUIContent _networkGC;
        private GUIContent _helpGC;
        private Vector2 _moduleListScroll;
        private Vector2 _moduleScroll;

        /// <summary>
        /// 当前选中的模块
        /// </summary>
        private Module CurrentModule
        {
            get
            {
                return _currentModule;
            }
            set
            {
                _currentModule = value;
                if (_currentModule != null)
                {
                    _currentModuleReadme = _modulesLibrary.GetReadMeFile(_currentModule);
                    _currentModuleLICENSE = _modulesLibrary.GetLICENSEFile(_currentModule);
                }
                else
                {
                    _currentModuleReadme = null;
                    _currentModuleLICENSE = null;
                }
            }
        }

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
            CurrentModule = null;
            _currentEditModule = null;
            _isCreateModule = false;
            _isEditModule = false;
            _inputModuleLocalPath = "";
            _inputModuleRemotePath = "";
            _gitBash = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Texture/GitBash.png");
            _github = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Texture/Github.png");
            _gitee = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Texture/Gitee.png");
            _moduleGC = new GUIContent();
            _moduleGC.image = EditorGUIUtility.IconContent("Folder Icon").image;
            _downloadedGC = new GUIContent();
            _downloadedGC.image = EditorGUIUtility.IconContent("TestPassed").image;
            _noDownloadedGC = new GUIContent();
            _noDownloadedGC.image = EditorGUIUtility.IconContent("TestFailed").image;
            _gitBashGC = new GUIContent();
            _gitBashGC.image = _gitBash;
            _gitBashGC.text = "Git Bash Here";
            _githubGC = new GUIContent();
            _githubGC.image = _github;
            _giteeGC = new GUIContent();
            _giteeGC.image = _gitee;
            _networkGC = new GUIContent();
            _networkGC.image = EditorGUIUtility.IconContent("BuildSettings.Web.Small").image;
            _helpGC = new GUIContent();
            _helpGC.image = EditorGUIUtility.IconContent("_Help").image;
            _helpGC.tooltip = "Help";
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

            EventHandle();
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
                    CurrentModule = null;
                    _currentEditModule = null;
                });
                gm.AddItem(new GUIContent("In Project"), _showModuleType == ModuleType.InProject, () =>
                {
                    _showModuleType = ModuleType.InProject;
                    CurrentModule = null;
                    _currentEditModule = null;
                });
                gm.ShowAsContext();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Git Bash", EditorStyles.toolbarButton))
            {
                GitBashWindow.OpenWindow(this);
            }
            if (GUILayout.Button("Credentials", EditorStyles.toolbarButton))
            {
                CredentialsProviderWindow.OpenWindow(this, _modulesLibrary.SetCredentials);
            }
            if (GUILayout.Button(_helpGC, "IconButton"))
            {
                Application.OpenURL("https://wanderer.blog.csdn.net/article/details/109488065");
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// 所有模块列表GUI
        /// </summary>
        private void OnModuleListGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(300));

            _moduleListScroll = GUILayout.BeginScrollView(_moduleListScroll);

            for (int i = 0; i < _modulesLibrary.Modules.Count; i++)
            {
                if (ModuleIsShow(_modulesLibrary.Modules[i]))
                {
                    if (CurrentModule == _modulesLibrary.Modules[i]) GUILayout.BeginHorizontal("InsertionMarker");
                    else GUILayout.BeginHorizontal();

                    GUI.color = _modulesLibrary.Modules[i].IsLocalExist ? Color.white : Color.gray;
                    _moduleGC.text = _modulesLibrary.Modules[i].Name;
                    if (GUILayout.Button(_moduleGC, EditorStyles.label, GUILayout.Width(250), GUILayout.Height(24)))
                    {
                        if (CurrentModule == _modulesLibrary.Modules[i])
                        {
                            CurrentModule = null;
                        }
                        else
                        {
                            CurrentModule = _modulesLibrary.Modules[i];
                        }
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
                _inputModuleLocalPath = EditorGUILayout.TextField(_inputModuleLocalPath);
                if (GUILayout.Button("Browse", "MiniButton", GUILayout.Width(60)))
                {
                    string path = EditorUtility.OpenFolderPanel("Select Local Path", Application.dataPath, "");
                    if (!string.IsNullOrEmpty(path))
                    {
                        _inputModuleLocalPath = path;
                        GUI.FocusControl(null);
                    }
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Label("Remote", GUILayout.Width(60));
                _inputModuleRemotePath = EditorGUILayout.TextField(_inputModuleRemotePath);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Create", "ButtonLeft"))
                {
                    _modulesLibrary.CreateNullModule(_inputModuleLocalPath, _inputModuleRemotePath);
                    _isCreateModule = false;
                }
                if (GUILayout.Button("Cancel", "ButtonRight"))
                {
                    _isCreateModule = false;
                }
                GUILayout.EndHorizontal();

                GUILayout.EndVertical();
            }
            else if (_isEditModule)
            {
                if (_currentEditModule == null)
                {
                    _isEditModule = false;
                }
                else
                {
                    GUILayout.BeginVertical("Box");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Name", GUILayout.Width(60));
                    GUILayout.Label(_currentEditModule.Name);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Local", GUILayout.Width(60));
                    _inputModuleLocalPath = EditorGUILayout.TextField(_inputModuleLocalPath);
                    if (GUILayout.Button("Browse", "MiniButton", GUILayout.Width(60)))
                    {
                        string path = EditorUtility.OpenFolderPanel("Select Local Path", Application.dataPath, "");
                        if (!string.IsNullOrEmpty(path))
                        {
                            _inputModuleLocalPath = path;
                            GUI.FocusControl(null);
                        }
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Remote", GUILayout.Width(60));
                    _inputModuleRemotePath = EditorGUILayout.TextField(_inputModuleRemotePath);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button("Edit", "ButtonLeft"))
                    {
                        _currentEditModule.Path = _inputModuleLocalPath;
                        _currentEditModule.RemotePath = _inputModuleRemotePath;
                        _currentEditModule.RefreshState();
                        _isEditModule = false;
                        _currentEditModule = null;
                    }
                    if (GUILayout.Button("Cancel", "ButtonRight"))
                    {
                        _isEditModule = false;
                        _currentEditModule = null;
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.EndVertical();
                }
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Create", "ButtonLeft"))
            {
                _isCreateModule = true;
                _isEditModule = false;
                _currentEditModule = null;
            }
            if (GUILayout.Button("Open", "ButtonMid"))
            {
                string path = EditorUtility.OpenFolderPanel("Open Module", Application.dataPath, "");
                if (!string.IsNullOrEmpty(path))
                {
                    _modulesLibrary.OpenModule(path);
                }
            }
            GUI.enabled = CurrentModule != null && !CurrentModule.IsLocalExist;
            if (GUILayout.Button("Edit", "ButtonMid"))
            {
                _isCreateModule = false;
                _isEditModule = true;
                _currentEditModule = CurrentModule;
                _inputModuleLocalPath = _currentEditModule.Path;
                _inputModuleRemotePath = _currentEditModule.RemotePath;
            }
            GUI.enabled = CurrentModule != null;
            if (GUILayout.Button("Remove", "ButtonMid"))
            {
                _modulesLibrary.RemoveModule(CurrentModule);
                CurrentModule = null;
                _currentEditModule = null;
            }
            GUI.enabled = true;
            GUI.backgroundColor = Color.yellow;
            if (GUILayout.Button("Update All", "ButtonRight"))
            {
                if (EditorUtility.DisplayDialog("Update All", "Are you sure you want to update all modules?", "Yes", "No"))
                {
                    _modulesLibrary.PullAll(() => { AssetDatabase.Refresh(); });
                }
            }
            GUI.backgroundColor = Color.white;
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.EndVertical();
        }
        /// <summary>
        /// 模块GUI
        /// </summary>
        private void OnModuleGUI()
        {
            if (CurrentModule == null)
                return;

            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(CurrentModule.Name, "LargeBoldLabel");
            GUILayout.Space(10);
            GUI.color = Color.cyan;
            GUILayout.Label("Branch: [" + CurrentModule.BranchName + "]", "LargeBoldLabel");
            GUI.color = Color.white;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUI.enabled = CurrentModule.IsLocalExist;
            if (GUILayout.Button("Local", "ButtonLeft"))
            {
                ProcessStartInfo psi = new ProcessStartInfo(CurrentModule.Path);
                Process.Start(psi);
            }
            GUI.enabled = CurrentModule.IsRemoteExist;
            if (GUILayout.Button("Remote", "ButtonRight"))
            {
                Application.OpenURL(CurrentModule.RemotePath);
            }
            GUI.enabled = !CurrentModule.IsLocalExist && CurrentModule.IsRemoteExist;
            if (GUILayout.Button("Download", "ButtonLeft"))
            {
                _modulesLibrary.Clone(CurrentModule, () => { AssetDatabase.Refresh(); });
            }
            GUI.enabled = CurrentModule.IsLocalExist && CurrentModule.IsRemoteExist;
            if (GUILayout.Button("Update", "ButtonRight"))
            {
                _modulesLibrary.Pull(CurrentModule, () => { AssetDatabase.Refresh(); });
            }
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button(_gitBashGC))
            {
                string gitBashPath = EditorPrefs.GetString(Utility.GitBashPath, "");
                if (string.IsNullOrEmpty(gitBashPath) || !File.Exists(gitBashPath))
                {
                    Utility.LogError("Open GitBash failed! please set the GitBash.exe path!");
                    GitBashWindow.OpenWindow(this);
                }
                else
                {
                    Process process = new Process();
                    process.StartInfo = new ProcessStartInfo(gitBashPath, "\"--cd=" + CurrentModule.Path + "\"");
                    process.Start();
                }
            }
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;
            if (_currentModuleLICENSE != null)
            {
                GUI.backgroundColor = Color.yellow;
                if (GUILayout.Button("LICENSE"))
                {
                    EditorGUIUtility.PingObject(_currentModuleLICENSE);
                }
                GUI.backgroundColor = Color.white;
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Local Path", GUILayout.Width(80), GUILayout.Height(20));
            GUILayout.Label(CurrentModule.Path, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Remote Path", GUILayout.Width(80), GUILayout.Height(20));
            GUIContent remoteGC = GetRemoteTypeGC(CurrentModule);
            remoteGC.text = CurrentModule.RemotePath;
            GUILayout.Label(remoteGC, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            if (CurrentModule.IsLocalExist)
            {
                if (_currentModuleReadme != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("README.md", "LargeBoldLabel");
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();

                    GUILayout.Space(10);

                    _moduleScroll = GUILayout.BeginScrollView(_moduleScroll, "HelpBox");
                    GUILayout.Label(_currentModuleReadme.text);
                    GUILayout.EndScrollView();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("There is no this module locally! please download to local first.", MessageType.Warning);
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
        /// 获取模块远端存储库类型对应的GC
        /// </summary>
        /// <param name="module">模块</param>
        /// <returns>模块GC</returns>
        private GUIContent GetRemoteTypeGC(Module module)
        {
            if (module.RemoteType == RemoteRepositoryType.Network)
                return _networkGC;
            if (module.RemoteType == RemoteRepositoryType.Github)
                return _githubGC;
            if (module.RemoteType == RemoteRepositoryType.Gitee)
                return _giteeGC;
            return _networkGC;
        }

        /// <summary>
        /// 事件处理
        /// </summary>
        private void EventHandle()
        {
            int startIndex;
            if (Event.current != null)
            {
                switch (Event.current.rawType)
                {
                    case EventType.KeyDown:
                        switch (Event.current.keyCode)
                        {
                            case KeyCode.DownArrow:
                                startIndex = CurrentModule != null ? (_modulesLibrary.Modules.IndexOf(CurrentModule) + 1) : 0;
                                for (int i = startIndex; i < _modulesLibrary.Modules.Count; i++)
                                {
                                    if (ModuleIsShow(_modulesLibrary.Modules[i]))
                                    {
                                        CurrentModule = _modulesLibrary.Modules[i];
                                        GUI.FocusControl(null);
                                        GUI.changed = true;
                                        return;
                                    }
                                }
                                break;
                            case KeyCode.UpArrow:
                                startIndex = CurrentModule != null ? (_modulesLibrary.Modules.IndexOf(CurrentModule) - 1) : (_modulesLibrary.Modules.Count - 1);
                                for (int i = startIndex; i >= 0; i--)
                                {
                                    if (ModuleIsShow(_modulesLibrary.Modules[i]))
                                    {
                                        CurrentModule = _modulesLibrary.Modules[i];
                                        GUI.FocusControl(null);
                                        GUI.changed = true;
                                        return;
                                    }
                                }
                                break;
                        }
                        break;
                }
            }
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