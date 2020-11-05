using LibGit2Sharp;
using System;
using System.IO;
using UnityEditor;

namespace HT.ModuleManager
{
    /// <summary>
    /// 模块
    /// </summary>
    [Serializable]
    internal sealed class Module : IDisposable
    {
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 模块本地路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 模块远端路径
        /// </summary>
        public string RemotePath;
        /// <summary>
        /// 是否存在本地模块
        /// </summary>
        public bool IsLocalExist;
        /// <summary>
        /// 是否存在远端模块
        /// </summary>
        public bool IsRemoteExist;
        /// <summary>
        /// 远端存储库类型
        /// </summary>
        public RemoteRepositoryType RemoteType;
        
        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="path">模块的本地路径</param>
        /// <param name="remotePath">模块的远端路径</param>
        public Module(string path, string remotePath)
        {
            Name = path.Substring(path.LastIndexOf("/") + 1);
            Path = path;
            RemotePath = remotePath;

            RefreshState();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {

        }
        /// <summary>
        /// 刷新状态
        /// </summary>
        public void RefreshState()
        {
            IsLocalExist = Directory.Exists(Path) && Repository.IsValid(Path);
            if (IsLocalExist)
            {
                using (Repository repository = new Repository(Path))
                {
                    if (repository != null)
                    {
                        if (string.IsNullOrEmpty(RemotePath))
                        {
                            RemotePath = repository.GetFirstRemotePath();
                        }
                    }
                }
            }
            IsRemoteExist = !string.IsNullOrEmpty(RemotePath);
            RemoteType = GetRemoteType(RemotePath);
        }
        /// <summary>
        /// 克隆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        public void Clone(string userName, string email, string password)
        {
            if (!IsLocalExist && IsRemoteExist)
            {
                try
                {
                    string path = Repository.Clone(RemotePath, Path, GetCloneOptions(userName, email, password));
                    Utility.LogInfo(string.Format("{0} clone succeed! cloned to path: {1}", Name, path));
                }
                catch (Exception e)
                {
                    Utility.LogError(string.Format("{0} clone failed! {1}", Name, e.Message));
                }
            }
        }
        /// <summary>
        /// 拉取
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        public void Pull(string userName, string email, string password)
        {
            if (IsLocalExist && IsRemoteExist)
            {
                using (Repository repository = new Repository(Path))
                {
                    if (repository != null)
                    {
                        try
                        {
                            MergeResult result = Commands.Pull(repository, GetSignature(userName, email), GetPullOptions(userName, email, password));

                            if (result.Status == MergeStatus.UpToDate || result.Status == MergeStatus.FastForward || result.Status == MergeStatus.NonFastForward)
                            {
                                if (result.Commit != null)
                                {
                                    Utility.LogInfo(string.Format("{0} pull succeed! up to date Commit: {1}", Name, result.Commit.Message));
                                }
                                else
                                {
                                    Utility.LogInfo(string.Format("{0} pull succeed! repository already up to date!", Name));
                                }
                            }
                            else
                            {
                                if (result.Commit != null)
                                {
                                    Utility.LogWarning(string.Format("{0} pull succeed! up to date Commit: {1}, but there are some conflicts!", Name, result.Commit.Message));
                                }
                                else
                                {
                                    Utility.LogError(string.Format("{0} pull failed! There have been some changes in the repository! please restore or submit first!", Name));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Utility.LogError(string.Format("{0} pull failed! {1}", Name, e.Message));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 生成Clone参数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>Clone参数</returns>
        private CloneOptions GetCloneOptions(string userName, string email, string password)
        {
            FetchOptions fetchOptions = new FetchOptions();
            fetchOptions.OnProgress = (output) =>
            {
                EditorUtility.DisplayProgressBar("Fetch", output, 0);
                return true;
            };

            CloneOptions cloneOptions = new CloneOptions();
            cloneOptions.OnProgress = (output) =>
            {
                EditorUtility.DisplayProgressBar("Clone", output, 0);
                return true;
            };
            cloneOptions.OnCheckoutProgress = (path, completedSteps, totalSteps) =>
            {
                EditorUtility.DisplayProgressBar("Checkout", path, (float)completedSteps / totalSteps);
            };
            cloneOptions.FetchOptions = fetchOptions;
            cloneOptions.CredentialsProvider = (url, usernameFromUrl, types) =>
            {
                return new UsernamePasswordCredentials { Username = userName, Password = password };
            };

            return cloneOptions;
        }
        /// <summary>
        /// 生成Pull参数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <param name="password">密码</param>
        /// <returns>Pull参数</returns>
        private PullOptions GetPullOptions(string userName, string email, string password)
        {
            FetchOptions fetchOptions = new FetchOptions();
            fetchOptions.OnProgress = (output) =>
            {
                EditorUtility.DisplayProgressBar("Fetch", output, 0);
                return true;
            };
            fetchOptions.CredentialsProvider = (url, usernameFromUrl, types) =>
            {
                return new UsernamePasswordCredentials { Username = userName, Password = password };
            };

            MergeOptions mergeOptions = new MergeOptions();
            mergeOptions.OnCheckoutProgress = (path, completedSteps, totalSteps) =>
            {
                EditorUtility.DisplayProgressBar("Checkout", path, (float)completedSteps / totalSteps);
            };

            PullOptions pullOptions = new PullOptions();
            pullOptions.FetchOptions = fetchOptions;
            pullOptions.MergeOptions = mergeOptions;

            return pullOptions;
        }
        /// <summary>
        /// 生成Signature参数
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="email">邮箱</param>
        /// <returns>Signature参数</returns>
        private Signature GetSignature(string userName, string email)
        {
            return new Signature(userName, email, new DateTimeOffset(DateTime.Now));
        }
        /// <summary>
        /// 获取存储库类型
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>存储库类型</returns>
        private RemoteRepositoryType GetRemoteType(string path)
        {
            if (string.IsNullOrEmpty(path))
                return RemoteRepositoryType.Network;
            if (path.Contains("github.com"))
                return RemoteRepositoryType.Github;
            if (path.Contains("gitee.com"))
                return RemoteRepositoryType.Gitee;
            return RemoteRepositoryType.Network;
        }
    }

    /// <summary>
    /// 远端存储库类型
    /// </summary>
    internal enum RemoteRepositoryType
    {
        Network,
        Github,
        Gitee
    }
}