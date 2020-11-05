using LibGit2Sharp;
using System.Collections.Generic;
using UnityEngine;

namespace HT.ModuleManager
{
    /// <summary>
    /// 模块管理器实用工具
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// 模块库的用户名Key
        /// </summary>
        public static readonly string UserNameKey = "HT.ModuleManager.UserName";

        /// <summary>
        /// 模块库的邮箱Key
        /// </summary>
        public static readonly string EmailKey = "HT.ModuleManager.Email";

        /// <summary>
        /// 模块库的密码Key
        /// </summary>
        public static readonly string PasswordKey = "HT.ModuleManager.Password";

        /// <summary>
        /// 打印普通日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogInfo(string content)
        {
            Debug.Log("<b><color=cyan>[Module Manager]</color></b> " + content);
        }

        /// <summary>
        /// 打印警告日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogWarning(string content)
        {
            Debug.LogWarning("<b><color=yellow>[Module Manager]</color></b> " + content);
        }

        /// <summary>
        /// 打印错误日志
        /// </summary>
        /// <param name="content">日志内容</param>
        public static void LogError(string content)
        {
            Debug.LogError("<b><color=red>[Module Manager]</color></b> " + content);
        }

        /// <summary>
        /// 获取第一个远端路径
        /// </summary>
        /// <param name="repository">模块仓库</param>
        /// <returns>远端路径</returns>
        public static string GetFirstRemotePath(this Repository repository)
        {
            IEnumerator<Remote> enumerator = repository.Network.Remotes.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current.Url;
            }
            return null;
        }
    }
}