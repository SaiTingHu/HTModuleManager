using System.Collections.Generic;
using System.Text;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown解析器
    /// </summary>
    internal sealed class MarkdownParser
    {
        /// <summary>
        /// 解析文档
        /// </summary>
        /// <param name="assetPath">文档资源路径</param>
        /// <param name="content">文档内容</param>
        /// <param name="isHideLoadFailImage">是否隐藏加载失败的图像</param>
        /// <returns>文档对象</returns>
        public MarkdownDocument Parse(string assetPath, string content, bool isHideLoadFailImage)
        {
            if (string.IsNullOrEmpty(content))
                return null;

            string[] lines = content.Split('\n');
            if (lines.Length > 0)
            {
                MarkdownDocument document = new MarkdownDocument(assetPath, isHideLoadFailImage);
                StringBuilder builder = new StringBuilder();
                bool isFragment = false;
                for (int i = 0; i < lines.Length; i++)
                {
                    if (isFragment)
                    {
                        if (lines[i].StartsWith("```"))
                        {
                            isFragment = false;
                            if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
                            document.AddContainer(ParseFragment(builder.ToString()));
                            builder.Clear();
                        }
                        else
                        {
                            builder.Append(lines[i]);
                            builder.Append('\n');
                        }
                    }
                    else
                    {
                        if (lines[i].StartsWith("```"))
                        {
                            isFragment = true;
                        }
                        else
                        {
                            document.AddContainer(ParseLine(lines[i]));
                        }
                    }                   
                }
                if (isFragment)
                {
                    if (builder.Length > 0) builder.Remove(builder.Length - 1, 1);
                    document.AddContainer(ParseFragment(builder.ToString()));
                    builder.Clear();
                }
                return document;
            }
            return null;
        }

        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="line">行内容</param>
        private MarkdownContainer ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            List<char> lineChars = line.AsList();
            MarkdownContainer container = new MarkdownContainer(ParseContainerType(lineChars));
            List<char> keywordsStart = new List<char>();
            List<char> text = new List<char>();
            List<char> keywordsEnd = new List<char>();
            for (int i = 0; i < lineChars.Count; i++)
            {
                char c = lineChars[i];
                if (c == '*' || c == '_' || c == '[' || c == ']' || c == '!')
                {
                    if (text.Count != 0)
                    {
                        if (keywordsStart.Count != 0)
                        {
                            keywordsEnd.Add(c);
                        }
                        else
                        {
                            container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                            i -= 1;
                            keywordsStart.Clear();
                            text.Clear();
                            keywordsEnd.Clear();
                        }
                    }
                    else
                    {
                        keywordsStart.Add(c);
                    }
                }
                else
                {
                    if (keywordsEnd.Count > 0)
                    {
                        //图像超链接
                        if (keywordsStart.StartWith("[![") && keywordsEnd.EndWith(']'))
                        {
                            int index1 = lineChars.IndexOfChar(')', i);
                            int index2 = (index1 != -1 && (index1 + 1 < lineChars.Count)) ? (index1 + 1) : -1;
                            int index3 = (index2 != -1 && (index2 + 1 < lineChars.Count)) ? (index2 + 1) : -1;
                            int index4 = (index3 != -1) ? lineChars.IndexOfChar(')', index3) : -1;
                            if (c == '(' && index1 != -1 && index2 != -1 && index3 != -1 && index4 != -1 && lineChars[index2] == ']' && lineChars[index3] == '(')
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd, lineChars.SubList(i, index1 - i + 1), lineChars.SubList(index3, index4 - index3 + 1)));
                                i = index4;
                            }
                            else
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                                i -= 1;
                            }
                        }
                        //图像
                        else if (keywordsStart.StartWith("![") && keywordsEnd.EndWith(']'))
                        {
                            int index = lineChars.IndexOfChar(')', i);
                            if (c == '(' && index != -1)
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd, lineChars.SubList(i, index - i + 1)));
                                i = index;
                            }
                            else
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                                i -= 1;
                            }
                        }
                        //文本超链接
                        else if (keywordsStart.StartWith('[') && keywordsEnd.EndWith(']'))
                        {
                            int index = lineChars.IndexOfChar(')', i);
                            if (c == '(' && index != -1)
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd, lineChars.SubList(i, index - i + 1)));
                                i = index;
                            }
                            else
                            {
                                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                                i -= 1;
                            }
                        }
                        else
                        {
                            container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                            i -= 1;
                        }
                        keywordsStart.Clear();
                        text.Clear();
                        keywordsEnd.Clear();
                    }
                    else
                    {
                        text.Add(c);
                    }
                }
            }
            if (keywordsStart.Count != 0 || text.Count != 0 || keywordsEnd.Count != 0)
            {
                container.AddBlock(ParseBlock(keywordsStart, text, keywordsEnd));
                keywordsStart.Clear();
                text.Clear();
                keywordsEnd.Clear();
            }
            return container;
        }
        /// <summary>
        /// 解析片段
        /// </summary>
        /// <param name="fragment">片段内容</param>
        private MarkdownContainer ParseFragment(string fragment)
        {
            if (string.IsNullOrEmpty(fragment))
                return null;

            MarkdownContainer container = new MarkdownContainer(ContainerType.Fragment);
            container.AddBlock(new BlockDefault(fragment));
            return container;
        }
        /// <summary>
        /// 解析块
        /// </summary>
        /// <param name="keywordsStart">块开始关键字</param>
        /// <param name="text">块内容</param>
        /// <param name="keywordsEnd">块结束关键字</param>
        /// <param name="url1">链接1（仅为链接型块时）</param>
        /// <param name="url2">链接2（仅为链接型块时）</param>
        private MarkdownBlock ParseBlock(List<char> keywordsStart, List<char> text, List<char> keywordsEnd, List<char> url1 = null, List<char> url2 = null)
        {
            if (url1 != null && url2 != null)
            {
                if (keywordsStart.StartWith("[![") && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveRange(0, 3);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    url1.RemoveAt(0);
                    url1.RemoveAt(url1.Count - 1);
                    url2.RemoveAt(0);
                    url2.RemoveAt(url2.Count - 1);
                    return new BlockImageLink(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd), url1.AsString(), url2.AsString());
                }
            }
            else if (url1 != null)
            {
                if (keywordsStart.StartWith("![") && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    url1.RemoveAt(0);
                    url1.RemoveAt(url1.Count - 1);
                    return new BlockImage(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd), url1.AsString());
                }
                if (keywordsStart.StartWith('[') && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    url1.RemoveAt(0);
                    url1.RemoveAt(url1.Count - 1);
                    return new BlockLink(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd), url1.AsString());
                }
            }
            else
            {
                if (keywordsStart.StartWith("**") && keywordsEnd.EndWith("**"))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveRange(keywordsEnd.Count - 2, 2);
                    return new BlockAsteriskTwo(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith('*') && keywordsEnd.EndWith('*'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    return new BlockAsterisk(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith("__") && keywordsEnd.EndWith("__"))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveRange(keywordsEnd.Count - 2, 2);
                    return new BlockUnderlineTwo(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith('_') && keywordsEnd.EndWith('_'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    return new BlockUnderline(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd));
                }
            }
            return new BlockDefault(MarkdownUtility.AsString(keywordsStart, text, keywordsEnd));
        }
        /// <summary>
        /// 解析行的容器类型
        /// </summary>
        /// <param name="lineChars">行内容</param>
        private ContainerType ParseContainerType(List<char> lineChars)
        {
            ContainerType type = ContainerType.Default;
            lineChars.Trim();
            if (lineChars.StartWith("# "))
            {
                type = ContainerType.Title1;
                lineChars.RemoveRange(0, 2);
            }
            else if (lineChars.StartWith("## "))
            {
                type = ContainerType.Title2;
                lineChars.RemoveRange(0, 3);
            }
            else if (lineChars.StartWith("### "))
            {
                type = ContainerType.Title3;
                lineChars.RemoveRange(0, 4);
            }
            else if (lineChars.StartWith("#### "))
            {
                type = ContainerType.Title4;
                lineChars.RemoveRange(0, 5);
            }
            else if (lineChars.StartWith("##### "))
            {
                type = ContainerType.Title5;
                lineChars.RemoveRange(0, 6);
            }
            else if (lineChars.StartWith("###### "))
            {
                type = ContainerType.Title6;
                lineChars.RemoveRange(0, 7);
            }
            else if (lineChars.StartWith("> "))
            {
                type = ContainerType.Quote;
                lineChars.RemoveRange(0, 2);
            }
            else if (lineChars.StartWith("- [ ] "))
            {
                type = ContainerType.NoCheckbox;
                lineChars.RemoveRange(0, 6);
            }
            else if (lineChars.StartWith("- [x] "))
            {
                type = ContainerType.Checkedbox;
                lineChars.RemoveRange(0, 6);
            }
            else if (lineChars.StartWith("- ") || lineChars.StartWith("+ ") || lineChars.StartWith("* "))
            {
                type = ContainerType.List;
                lineChars.RemoveRange(0, 2);
            }
            lineChars.Trim();
            return type;
        }
    }
}