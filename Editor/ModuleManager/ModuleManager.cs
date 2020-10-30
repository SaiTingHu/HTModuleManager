using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// 模块管理器
    /// </summary>
    internal sealed class ModuleManager : EditorWindow
    {
        [MenuItem("HT/Module Manager", priority = 2000)]
        private static void OpenWindow()
        {
            ModuleManager window = GetWindow<ModuleManager>();
            window.titleContent.text = "Module Manager";
            window.Show();
        }
        
        private LibGit2 _libGit2;
        private LibGit2Repository _currentRepository;
        private ModuleType _showModuleType = ModuleType.InProject;
        private Texture2D _github;
        private GUIContent _repositoryGC;
        private GUIContent _downloadedGC;
        private GUIContent _noDownloadedGC;
        private GUIContent _githubGC;
        private GUIContent _networkGC;
        private Vector2 _scroll;
        
        private void OnEnable()
        {
            if (_libGit2 == null)
            {
                _libGit2 = new LibGit2(GetNativeModulesDefine());
            }
            else
            {
                _libGit2.RefreshState();
            }
            _currentRepository = null;
            _github = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/HTModuleManager/Editor/Texture/Github.png");
            _repositoryGC = new GUIContent();
            _repositoryGC.image = EditorGUIUtility.IconContent("Folder Icon").image;
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

            OnRepositoriesListGUI();

            GUILayout.Space(5);

            GUILayout.Box("", "DopesheetBackground", GUILayout.Width(5), GUILayout.ExpandHeight(true));

            GUILayout.Space(5);

            OnRepositoryGUI();

            GUILayout.EndHorizontal();
        }
        private void OnDestroy()
        {
            if (_libGit2 != null)
            {
                _libGit2.Dispose();
                _libGit2 = null;
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
                    _currentRepository = null;
                });
                gm.AddItem(new GUIContent("In Project"), _showModuleType == ModuleType.InProject, () =>
                {
                    _showModuleType = ModuleType.InProject;
                    _currentRepository = null;
                });
                gm.ShowAsContext();
            }
            if (GUILayout.Button("Open Repository", EditorStyles.toolbarPopup))
            {
                string path = EditorUtility.OpenFolderPanel("Open Repository", Application.dataPath, "");
                if (path != "")
                {
                    _libGit2.OpenRepository(path);
                }
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Pull All", EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Pull All", "Are you sure you want pull all Repositories?", "Yes", "No"))
                {
                    for (int i = 0; i < _libGit2.Repositories.Count; i++)
                    {
                        _libGit2.Repositories[i].Pull();
                    }
                    AssetDatabase.Refresh();
                }
            }
            GUILayout.EndHorizontal();
        }
        /// <summary>
        /// 所有模块仓库列表GUI
        /// </summary>
        private void OnRepositoriesListGUI()
        {
            GUILayout.BeginVertical(GUILayout.Width(300));
            _scroll = GUILayout.BeginScrollView(_scroll);

            for (int i = 0; i < _libGit2.Repositories.Count; i++)
            {
                if (ModuleIsShow(_libGit2.Repositories[i]))
                {
                    if (_currentRepository == _libGit2.Repositories[i]) GUILayout.BeginHorizontal("InsertionMarker");
                    else GUILayout.BeginHorizontal();

                    GUI.color = _libGit2.Repositories[i].IsLocalExist ? Color.white : Color.gray;
                    _repositoryGC.text = _libGit2.Repositories[i].Name;
                    if (GUILayout.Button(_repositoryGC, EditorStyles.label, GUILayout.Height(24)))
                    {
                        _currentRepository = _libGit2.Repositories[i];
                        GUI.FocusControl(null);
                    }
                    GUI.color = Color.white;

                    GUILayout.FlexibleSpace();

                    GUILayout.Label(_libGit2.Repositories[i].IsLocalExist ? _downloadedGC : _noDownloadedGC);

                    GUILayout.EndHorizontal();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        /// <summary>
        /// 模块仓库GUI
        /// </summary>
        private void OnRepositoryGUI()
        {
            if (_currentRepository == null)
                return;

            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label(_currentRepository.Name, "LargeBoldLabel");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUI.enabled = _currentRepository.IsLocalExist;
            if (GUILayout.Button("Local", "ButtonLeft"))
            {
                ProcessStartInfo psi = new ProcessStartInfo(_currentRepository.Path);
                Process.Start(psi);
            }
            GUI.enabled = _currentRepository.IsRemoteExist;
            if (GUILayout.Button("Remote", "ButtonRight"))
            {
                Application.OpenURL(_currentRepository.RemotePath);
            }
            GUI.enabled = !_currentRepository.IsLocalExist && _currentRepository.IsRemoteExist;
            if (GUILayout.Button("Clone", "ButtonLeft"))
            {
                _currentRepository.Clone();
                AssetDatabase.Refresh();
            }
            GUI.enabled = _currentRepository.IsLocalExist && _currentRepository.IsRemoteExist;
            if (GUILayout.Button("Pull", "ButtonRight"))
            {
                _currentRepository.Pull();
                AssetDatabase.Refresh();
            }
            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Local Path", GUILayout.Width(80), GUILayout.Height(20));
            GUILayout.Label(_currentRepository.Path, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Remote Path", GUILayout.Width(80), GUILayout.Height(20));
            _githubGC.text = _networkGC.text = _currentRepository.RemotePath;
            GUILayout.Label(_currentRepository.IsGithub ? _githubGC : _networkGC, "Badge", GUILayout.Height(20));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            if (!_currentRepository.IsLocalExist)
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
            string[] repositories = File.ReadAllLines(Application.dataPath + "/HTModuleManager/Editor/ModuleManager/NativeModules.txt");
            for (int i = 0; i < repositories.Length; i++)
            {
                repositories[i] = Application.dataPath + "/" + repositories[i];
            }
            return repositories;
        }
        /// <summary>
        /// 模块仓库是否显示
        /// </summary>
        /// <param name="repository">模块仓库</param>
        /// <returns>是否显示</returns>
        private bool ModuleIsShow(LibGit2Repository repository)
        {
            return _showModuleType == ModuleType.AllModule ? true : repository.IsLocalExist;
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