using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown表格【|---|】
    /// </summary>
    internal class BlockTable : MarkdownBlock
    {
        private static readonly Regex TableRegex = new Regex(@"[:]?[-]{3,}[:]?", RegexOptions.Singleline);
        private static readonly float Gap = 8;

        private List<Row> _rows = new List<Row>();
        private string _noTableText;

        public BlockTable(string text, List<string> lines) : base(text)
        {
            List<int> aligns = new List<int>();
            List<string> cells = new List<string>();
            for (int i = 0; i < lines.Count; i++)
            {
                cells.Clear();
                string[] strs = lines[i].Split('|');
                for (int j = 0; j < strs.Length; j++)
                {
                    string str = strs[j].Trim();
                    if (!string.IsNullOrEmpty(str))
                    {
                        cells.Add(str);
                    }
                }

                if (cells.Count > 0)
                {
                    if (IsTableSign(cells))
                    {
                        aligns.Clear();
                        for (int m = 0; m < cells.Count; m++)
                        {
                            if (cells[m].StartsWith(':') && cells[m].EndsWith(':')) aligns.Add(0);
                            else if (cells[m].StartsWith(':')) aligns.Add(-1);
                            else if (cells[m].EndsWith(':')) aligns.Add(1);
                            else aligns.Add(0);
                        }
                    }
                    else
                    {
                        bool isTitle = _rows.Count == 0;
                        _rows.Add(new Row(cells, isTitle));
                    }
                }
            }

            if (_rows.Count > 0)
            {
                for (int i = 0; i < _rows.Count; i++)
                {
                    _rows[i].SetAlign(aligns);
                }
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < lines.Count; i++)
                {
                    stringBuilder.Append(lines[i]);
                    stringBuilder.Append("\r\n");
                }
                _noTableText = stringBuilder.ToString();
            }
        }

        /// <summary>
        /// 绘制块
        /// </summary>
        public override void Draw(GUISkin skin)
        {
            if (_rows.Count > 0)
            {
                for (int i = 0; i < _rows.Count; i++)
                {
                    _rows[i].Draw(skin);
                }
            }
            else
            {
                GUILayout.Label(_noTableText);
            }
        }

        /// <summary>
        /// 是否为表格的标记行
        /// </summary>
        private bool IsTableSign(List<string> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                if (!TableRegex.IsMatch(cells[i]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 行
        /// </summary>
        private class Row
        {
            /// <summary>
            /// 所有的元素
            /// </summary>
            public List<Cell> Cells { get; private set; } = new List<Cell>();

            public Row(List<string> cells, bool isTitle)
            {
                for (int i = 0; i < cells.Count; i++)
                {
                    Cells.Add(new Cell(cells[i], 0, isTitle));
                }
            }

            /// <summary>
            /// 设置行中元素的对齐方式
            /// </summary>
            public void SetAlign(List<int> aligns)
            {
                for (int i = 0; i < Cells.Count && i < aligns.Count; i++)
                {
                    Cells[i].SetAlign(aligns[i]);
                }
            }
            /// <summary>
            /// 绘制行
            /// </summary>
            public void Draw(GUISkin skin)
            {
                float cellWidth = (EditorGUIUtility.currentViewWidth - (Cells.Count + 1) * Gap) / Cells.Count;

                GUILayout.BeginHorizontal();
                for (int i = 0; i < Cells.Count; i++)
                {
                    Cells[i].Draw(skin, cellWidth);
                }
                GUILayout.EndHorizontal();
            }
        }
        /// <summary>
        /// 行中的元素
        /// </summary>
        private class Cell
        {
            /// <summary>
            /// 元素内容
            /// </summary>
            public string Content { get; private set; }
            /// <summary>
            /// 对齐方式（-1：左对齐，0：居中对齐，1：右对齐）
            /// </summary>
            public int Align { get; private set; }
            /// <summary>
            /// 是否为标题
            /// </summary>
            public bool IsTitle { get; private set; }

            private TextAnchor _align;
            private FontStyle _fontStyle;
            private Color _bgColor;

            public Cell(string content, int align, bool isTitle)
            {
                Content = content;
                Align = align;
                IsTitle = isTitle;

                if (Align == -1) _align = TextAnchor.MiddleLeft;
                else if (Align == 0) _align = TextAnchor.MiddleCenter;
                else if (Align == 1) _align = TextAnchor.MiddleRight;
                _fontStyle = IsTitle ? FontStyle.Bold : FontStyle.Normal;
                _bgColor = IsTitle ? Color.black : Color.gray;
            }

            /// <summary>
            /// 设置元素的对齐方式
            /// </summary>
            public void SetAlign(int align)
            {
                Align = align;

                if (Align == -1) _align = TextAnchor.MiddleLeft;
                else if (Align == 0) _align = TextAnchor.MiddleCenter;
                else if (Align == 1) _align = TextAnchor.MiddleRight;
            }
            /// <summary>
            /// 绘制元素
            /// </summary>
            public void Draw(GUISkin skin, float width)
            {
                TextAnchor align = skin.box.alignment;
                FontStyle fontStyle = skin.box.fontStyle;
                Color color = GUI.backgroundColor;
                skin.box.alignment = _align;
                skin.box.fontStyle = _fontStyle;
                GUI.backgroundColor = _bgColor;

                GUILayout.Box(Content, GUILayout.Width(width));

                skin.box.alignment = align;
                skin.box.fontStyle = fontStyle;
                GUI.backgroundColor = color;
            }
        }
    }
}