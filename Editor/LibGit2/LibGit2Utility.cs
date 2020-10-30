using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// LibGit2实用工具
    /// </summary>
    public static class LibGit2Utility
    {
        /// <summary>
        /// 打印普通日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogInfo(string content)
        {
            Debug.Log("<b><color=cyan>[LibGit2]</color></b> " + content);
        }

        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogWarning(string content)
        {
            Debug.LogWarning("<b><color=yellow>[LibGit2]</color></b> " + content);
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogError(string content)
        {
            Debug.LogError("<b><color=red>[LibGit2]</color></b> " + content);
        }
    }
}