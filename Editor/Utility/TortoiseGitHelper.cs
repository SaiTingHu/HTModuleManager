using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// TortoiseGit助手
    /// </summary>
    public static class TortoiseGitHelper
    {
        /// <summary>
        /// 获取配置的TortoiseGit启动路径
        /// </summary>
        /// <returns></returns>
        public static string GetTortoiseGitPath()
        {
            return EditorPrefs.GetString(Utility.TortoiseGitPath, "");
        }
        /// <summary>
        /// 关于（根据配置路径启动TortoiseGit）
        /// </summary>
        public static void About()
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:about");
            process.Start();
        }
        /// <summary>
        /// 添加子模块到项目（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="localPath">存储库路径（本地）</param>
        /// <param name="remotePath">存储库路径（远端）</param>
        public static void AddToSubModule(string projectPath, string localPath, string remotePath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(projectPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + projectPath + "!");
                return;
            }

            string relativePath = "Assets" + localPath.Replace(Application.dataPath, "");
            relativePath = relativePath.Substring(0, relativePath.LastIndexOf('/'));
            GUIUtility.systemCopyBuffer = remotePath;
            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, $"/command:subadd /path:\"{relativePath}\"");
            process.StartInfo.WorkingDirectory = projectPath;
            process.Start();
        }
        /// <summary>
        /// 从项目中移除子模块（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        /// <param name="localPath">存储库路径（本地）</param>
        public static void RemoveSubModule(string projectPath, string localPath)
        {
            string path = localPath.Replace(projectPath + "/", "");

            StringBuilder sb = new StringBuilder();
            sb.Append("remove submodule steps:\r\n");
            sb.Append($"1.delete folder: <color=yellow>.git/modules/{path}</color>\r\n");
            sb.Append("2.modify config: <color=yellow>.git/config</color>\r\n");
            sb.Append($"3.delete folder: <color=yellow>{path}</color>\r\n");
            sb.Append("4.modify config: <color=yellow>.gitmodules</color>\r\n");
            sb.Append($"5.clear cache: <color=yellow>git rm --cached {path}</color>\r\n");
            sb.Append("6.commit: <color=yellow>git commit</color>\r\n");
            Utility.LogInfo(sb.ToString());
        }
        /// <summary>
        /// 更新所有子模块（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="projectPath">项目路径</param>
        public static void UpdateAllSubModule(string projectPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(projectPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + projectPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, $"/command:subupdate /path:\"{projectPath}\\.gitmodules\"");
            process.StartInfo.WorkingDirectory = projectPath;
            process.Start();
        }
        /// <summary>
        /// 提交存储库（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Commit(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:commit");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 拉取存储库（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Pull(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:pull");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 推送存储库（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Push(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:push");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 查看存储库日志（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Log(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:log");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 查看存储库状态（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Status(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:repostatus");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 查看存储库设置面板（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void Settings(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:settings");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 切换存储库分支（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void SwitchBranch(string repositoryPath)
        {
            string path = GetTortoiseGitPath();
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Utility.LogError("Open Tortoise Git failed! please set the TortoiseGitProc.exe path in ModuleManager window!");
                return;
            }

            if (!Directory.Exists(repositoryPath))
            {
                Utility.LogError("Open Tortoise Git failed! the Repository does not exist for path: " + repositoryPath + "!");
                return;
            }

            using Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:switch");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
    }
}