using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace HT.ModuleManager
{
    /// <summary>
    /// LibGit2库
    /// </summary>
    [Serializable]
    internal sealed class LibGit2 : IDisposable
    {
        /// <summary>
        /// 用于签名的用户名
        /// </summary>
        public static string Name = "SaiTingHu";
        /// <summary>
        /// 用于签名的邮箱
        /// </summary>
        public static string Email = "987947865@qq.com";

        /// <summary>
        /// 所有仓库
        /// </summary>
        public List<LibGit2Repository> Repositories = new List<LibGit2Repository>();

        /// <summary>
        /// LibGit2库
        /// </summary>
        /// <param name="defines">仓库的定义</param>
        public LibGit2(string[] defines)
        {
            for (int i = 0; i < defines.Length; i++)
            {
                string[] paths = defines[i].Split('|');
                CreateNullRepository(paths[0], paths[1]);
            }
        }
        
        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            DisposeAllRepository();
        }
        /// <summary>
        /// 刷新状态
        /// </summary>
        public void RefreshState()
        {
            for (int i = 0; i < Repositories.Count; i++)
            {
                Repositories[i].RefreshState();
            }
        }
        /// <summary>
        /// 打开一个仓库
        /// </summary>
        /// <param name="path">仓库的本地路径</param>
        public void OpenRepository(string path)
        {
            if (Repositories.Exists((repo) => { return repo.Path == path; }))
            {
                LibGit2Utility.LogError(string.Format("Open repository failed! {0} is already opend!", path));
                return;
            }

            if (Directory.Exists(path) && Repository.IsValid(path))
            {
                Repositories.Add(new LibGit2Repository(path, null));
            }
            else
            {
                LibGit2Utility.LogError(string.Format("Open repository failed! {0} is not a valid repository!", path));
            }
        }
        /// <summary>
        /// 创建一个空仓库
        /// </summary>
        /// <param name="path">仓库的本地路径</param>
        /// <param name="remotePath">远端仓库路径</param>
        public void CreateNullRepository(string path, string remotePath)
        {
            if (Repositories.Exists((repo) => { return repo.Path == path; }))
            {
                LibGit2Utility.LogError(string.Format("Open repository failed! {0} is already opend!", path));
                return;
            }

            Repositories.Add(new LibGit2Repository(path, remotePath));
        }
        /// <summary>
        /// 移除一个仓库
        /// </summary>
        /// <param name="repository">仓库</param>
        public void DisposeRepository(LibGit2Repository repository)
        {
            if (Repositories.Contains(repository))
            {
                repository.Dispose();
                Repositories.Remove(repository);
            }
        }
        /// <summary>
        /// 移除一个仓库
        /// </summary>
        /// <param name="repositoryIndex">仓库索引</param>
        public void DisposeRepository(int repositoryIndex)
        {
            if (repositoryIndex >= 0 && repositoryIndex < Repositories.Count)
            {
                Repositories[repositoryIndex].Dispose();
                Repositories.RemoveAt(repositoryIndex);
            }
        }
        /// <summary>
        /// 移除所有仓库
        /// </summary>
        public void DisposeAllRepository()
        {
            for (int i = 0; i < Repositories.Count; i++)
            {
                Repositories[i].Dispose();
            }
            Repositories.Clear();
        }
    }
}