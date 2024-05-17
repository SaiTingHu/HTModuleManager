using System.Collections.Generic;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown语法容器
    /// </summary>
    internal sealed class MarkdownContainer
    {
        private List<MarkdownBlock> _block;
        private ContainerType _containerType;
        private int _fontSize;
        private Color _contentColor;
        private Color _bgColor;
        private string _style;
        private GUIStyle _toggleStyle;

        /// <summary>
        /// 所属的文档
        /// </summary>
        public MarkdownDocument Document { get; set; }

        public MarkdownContainer(ContainerType type)
        {
            _block = new List<MarkdownBlock>();
            _containerType = type;
            switch (_containerType)
            {
                case ContainerType.Title1:
                    _fontSize = 40;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Title2:
                    _fontSize = 36;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Title3:
                    _fontSize = 32;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Title4:
                    _fontSize = 28;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Title5:
                    _fontSize = 24;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Title6:
                    _fontSize = 20;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.Quote:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.gray;
                    _style = "Box";
                    break;
                case ContainerType.List:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
                case ContainerType.NoCheckbox:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    _toggleStyle = "Toggle";
                    break;
                case ContainerType.Checkedbox:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    _toggleStyle = "Toggle";
                    break;
                case ContainerType.Fragment:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.black;
                    _style = "Box";
                    break;
                case ContainerType.Default:
                default:
                    _fontSize = 14;
                    _contentColor = Color.white;
                    _bgColor = Color.white;
                    _style = null;
                    break;
            }
        }

        /// <summary>
        /// 添加一个语法块对象到容器中
        /// </summary>
        /// <param name="block">语法块对象</param>
        public void AddBlock(MarkdownBlock block)
        {
            if (block == null)
                return;

            block.Container = this;
            _block.Add(block);
        }
        /// <summary>
        /// 绘制容器
        /// </summary>
        public void Draw(GUISkin skin)
        {
            skin.button.fontSize = _fontSize;
            skin.toggle.fontSize = _fontSize;
            skin.label.fontSize = _fontSize;
            GUI.contentColor = _contentColor;
            GUI.backgroundColor = _bgColor;
            GUI.enabled = true;

            if (string.IsNullOrEmpty(_style)) GUILayout.BeginHorizontal();
            else GUILayout.BeginHorizontal(_style);

            if (_containerType == ContainerType.Quote)
            {
                GUILayout.Space(10);
            }
            else if (_containerType == ContainerType.List)
            {
                GUILayout.Label("●");
            }
            else if (_containerType == ContainerType.NoCheckbox)
            {
                GUILayout.Space(25);
            }
            else if (_containerType == ContainerType.Checkedbox)
            {
                GUILayout.Space(25);
            }

            for (int i = 0; i < _block.Count; i++)
            {
                _block[i].Draw(skin);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            if (_containerType == ContainerType.Quote)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                rect.width = 10;
                GUI.Box(rect, "");
            }
            else if (_containerType == ContainerType.NoCheckbox)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                rect.x = 5;
                rect.width = 20;
                GUI.Toggle(rect, false, "", _toggleStyle);
            }
            else if (_containerType == ContainerType.Checkedbox)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                rect.x = 5;
                rect.width = 20;
                GUI.Toggle(rect, true, "", _toggleStyle);
            }
        }
    }

    /// <summary>
    /// 容器类型
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        /// 普通文本
        /// </summary>
        Default,
        /// <summary>
        /// 1级标题 [# ]
        /// </summary>
        Title1,
        /// <summary>
        /// 2级标题 [## ]
        /// </summary>
        Title2,
        /// <summary>
        /// 3级标题 [### ]
        /// </summary>
        Title3,
        /// <summary>
        /// 4级标题 [#### ]
        /// </summary>
        Title4,
        /// <summary>
        /// 5级标题 [##### ]
        /// </summary>
        Title5,
        /// <summary>
        /// 6级标题 [###### ]
        /// </summary>
        Title6,
        /// <summary>
        /// 引用 [> ]
        /// </summary>
        Quote,
        /// <summary>
        /// 列表 [- 、* 、+ ]
        /// </summary>
        List,
        /// <summary>
        /// 勾选框（未勾选） [- [ ] ]
        /// </summary>
        NoCheckbox,
        /// <summary>
        /// 勾选框（已勾选） [- [x] ]
        /// </summary>
        Checkedbox,
        /// <summary>
        /// 片段
        /// </summary>
        Fragment
    }
}