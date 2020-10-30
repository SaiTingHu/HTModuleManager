using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace HT.ModuleManager
{
    /// <summary>
    /// LibGit2仓库
    /// </summary>
    [Serializable]
    internal sealed class LibGit2Repository : IDisposable
    {
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 仓库本地路径
        /// </summary>
        public string Path;
        /// <summary>
        /// 远端仓库路径
        /// </summary>
        public string RemotePath;
        /// <summary>
        /// 是否存在本地仓库
        /// </summary>
        public bool IsLocalExist;
        /// <summary>
        /// 是否存在远端仓库
        /// </summary>
        public bool IsRemoteExist;
        /// <summary>
        /// 远端仓库是否为Github
        /// </summary>
        public bool IsGithub;

        /// <summary>
        /// LibGit2仓库
        /// </summary>
        /// <param name="path">仓库的本地路径</param>
        /// <param name="remotePath">远端仓库路径</param>
        public LibGit2Repository(string path, string remotePath)
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
            if (IsLocalExist && string.IsNullOrEmpty(RemotePath))
            {
                using (Repository repository = new Repository(Path))
                {
                    if (repository != null)
                    {
                        IEnumerator<Remote> enumerator = repository.Network.Remotes.GetEnumerator();
                        if (enumerator.MoveNext())
                        {
                            RemotePath = enumerator.Current.Url;
                        }
                    }
                }
            }
            IsRemoteExist = !string.IsNullOrEmpty(RemotePath);
            IsGithub = string.IsNullOrEmpty(RemotePath) ? false : RemotePath.Contains("github.com");
        }
        /// <summary>
        /// 克隆
        /// </summary>
        public void Clone()
        {
            if (!IsLocalExist && IsRemoteExist)
            {
                try
                {
                    string path = Repository.Clone(RemotePath, Path, GetCloneOptions());
                    LibGit2Utility.LogInfo(string.Format("{0} clone succeed! cloned to path: {1}", Name, path));
                }
                catch (Exception e)
                {
                    LibGit2Utility.LogError(string.Format("{0} clone failed! {1}", Name, e.Message));
                }
                finally
                {
                    EditorUtility.ClearProgressBar();
                }
            }
        }
        /// <summary>
        /// 拉取
        /// </summary>
        public void Pull()
        {
            if (IsLocalExist && IsRemoteExist)
            {
                using (Repository repository = new Repository(Path))
                {
                    if (repository != null)
                    {
                        try
                        {
                            MergeResult result = Commands.Pull(repository, new Signature(LibGit2.Name, LibGit2.Email, new DateTimeOffset(DateTime.Now)), GetPullOptions());

                            if (result.Status == MergeStatus.UpToDate || result.Status == MergeStatus.FastForward || result.Status == MergeStatus.NonFastForward)
                            {
                                if (result.Commit != null)
                                {
                                    LibGit2Utility.LogInfo(string.Format("{0} pull succeed! up to date Commit: {1}", Name, result.Commit.Message));
                                }
                                else
                                {
                                    LibGit2Utility.LogInfo(string.Format("{0} pull succeed! repository already up to date!", Name));
                                }
                            }
                            else
                            {
                                if (result.Commit != null)
                                {
                                    LibGit2Utility.LogWarning(string.Format("{0} pull succeed! up to date Commit: {1}, but there are some conflicts!", Name, result.Commit.Message));
                                }
                                else
                                {
                                    LibGit2Utility.LogError(string.Format("{0} pull failed! There have been some changes in the repository! please restore or submit first!", Name));
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LibGit2Utility.LogError(string.Format("{0} pull failed! {1}", Name, e.Message));
                        }
                        finally
                        {
                            EditorUtility.ClearProgressBar();
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 生成Clone参数
        /// </summary>
        /// <returns>Clone参数</returns>
        private CloneOptions GetCloneOptions()
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

            return cloneOptions;
        }
        /// <summary>
        /// 生成Pull参数
        /// </summary>
        /// <returns>Pull参数</returns>
        private PullOptions GetPullOptions()
        {
            FetchOptions fetchOptions = new FetchOptions();
            fetchOptions.OnProgress = (output) =>
            {
                EditorUtility.DisplayProgressBar("Fetch", output, 0);
                return true;
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
    }
}