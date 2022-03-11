using System.Diagnostics;
using System.IO;
using UnityEditor;

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

            Process process = new Process();
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

            Process process = new Process();
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

            Process process = new Process();
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

            Process process = new Process();
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

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:repostatus");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
        /// <summary>
        /// 查看存储库版本分支图（根据配置路径启动TortoiseGit）
        /// </summary>
        /// <param name="repositoryPath">存储库路径</param>
        public static void RevisionGraph(string repositoryPath)
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

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:revisiongraph");
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

            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(path, "/command:switch");
            process.StartInfo.WorkingDirectory = repositoryPath;
            process.Start();
        }
    }
}