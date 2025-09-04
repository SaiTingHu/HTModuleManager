using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown文档
    /// </summary>
    internal sealed class MarkdownDocument
    {
        /// <summary>
        /// 超链接块颜色
        /// </summary>
        public readonly Color LinkColor = new Color(0.298f, 0.494f, 1, 1);
        /// <summary>
        /// 对象块颜色
        /// </summary>
        public readonly Color ObjectColor = new Color(1f, 0.647f, 0, 1);
        /// <summary>
        /// 自定义块颜色
        /// </summary>
        public readonly Color CustomColor = new Color(0f, 1f, 1f, 1f);

        private List<MarkdownContainer> _containers;
        private Vector2 _scroll;
        private MethodInfo _customAction;
        private object[] _args;

        /// <summary>
        /// 文档资源路径
        /// </summary>
        public string AssetPath { get; private set; }
        /// <summary>
        /// 是否隐藏加载失败的图像
        /// </summary>
        public bool IsHideLoadFailImage { get; private set; }

        public MarkdownDocument(string assetPath, bool isHideLoadFailImage)
        {
            _containers = new List<MarkdownContainer>();
            _scroll = Vector2.zero;

            List<Type> types = MarkdownUtility.GetTypesInEditor();
            for (int i = 0; i < types.Count; i++)
            {
                if (_customAction == null)
                {
                    MethodInfo[] methods = types[i].GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    for (int j = 0; j < methods.Length; j++)
                    {
                        if (methods[j].IsDefined(typeof(BlockCustomActionAttribute), false) && methods[j].IsBlockCustomActionMethod())
                        {
                            _customAction = methods[j];
                            _args = new object[1];
                            break;
                        }
                    }
                }
            }

            AssetPath = assetPath;
            IsHideLoadFailImage = isHideLoadFailImage;
        }

        /// <summary>
        /// 添加一个容器对象到文档中
        /// </summary>
        /// <param name="container">容器对象</param>
        public void AddContainer(MarkdownContainer container)
        {
            if (container == null)
                return;

            container.Document = this;
            _containers.Add(container);
        }
        /// <summary>
        /// 绘制文档
        /// </summary>
        public void Draw(GUISkin skin)
        {
            GUILayout.BeginVertical();
            _scroll = GUILayout.BeginScrollView(_scroll);

            GUI.skin = skin;

            for (int i = 0; i < _containers.Count; i++)
            {
                _containers[i].Draw(skin);
            }

            GUI.skin = null;

            GUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
        /// <summary>
        /// 执行自定义操作
        /// </summary>
        /// <param name="args">参数</param>
        public void DoCustomAction(string args)
        {
            if (_customAction != null)
            {
                _args[0] = args;
                _customAction.Invoke(null, _args);
            }
        }
    }
}