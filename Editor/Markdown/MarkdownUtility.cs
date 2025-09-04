using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown实用工具
    /// </summary>
    internal static class MarkdownUtility
    {
        /// <summary>
        /// 裁剪字符数组前后的空白字符
        /// </summary>
        /// <param name="cs">字符数组</param>
        public static void Trim(this List<char> cs)
        {
            if (cs == null)
                return;

            for (int i = 0; i < cs.Count; i++)
            {
                if (char.IsWhiteSpace(cs[i]))
                {
                    cs.RemoveAt(i);
                    i -= 1;
                }
                else
                {
                    break;
                }
            }
            for (int i = cs.Count - 1; i >= 0; i--)
            {
                if (char.IsWhiteSpace(cs[i]))
                {
                    cs.RemoveAt(i);
                }
                else
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 字符数组是否由指定字符串开始
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="str">指定字符串</param>
        public static bool StartWith(this List<char> cs, string str)
        {
            if (cs == null || string.IsNullOrEmpty(str))
                return false;

            if (str.Length > cs.Count)
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != cs[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字符数组是否由指定字符串结束
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="str">指定字符串</param>
        public static bool EndWith(this List<char> cs, string str)
        {
            if (cs == null || string.IsNullOrEmpty(str))
                return false;

            if (str.Length > cs.Count)
                return false;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] != cs[cs.Count - str.Length + i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 字符数组是否由指定字符开始
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="c">指定字符</param>
        public static bool StartWith(this List<char> cs, char c)
        {
            if (cs == null)
                return false;

            if (cs.Count == 0)
                return false;

            if (cs[0] == c)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 字符数组是否由指定字符结束
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="c">指定字符</param>
        public static bool EndWith(this List<char> cs, char c)
        {
            if (cs == null)
                return false;

            if (cs.Count == 0)
                return false;

            if (cs[cs.Count - 1] == c)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 转换为string对象
        /// </summary>
        /// <param name="cs">字符数组</param>
        public static string AsString(this List<char> cs)
        {
            if (cs == null)
                return null;

            return new string(cs.ToArray());
        }
        /// <summary>
        /// 将多个字符数组合并转换为string对象
        /// </summary>
        /// <param name="cs1">字符数组1</param>
        /// <param name="cs2">字符数组2</param>
        public static string AsString(List<char> cs1, List<char> cs2)
        {
            if (cs1 == null && cs2 == null)
                return null;

            List<char> total = new List<char>();
            if (cs1 != null) total.AddRange(cs1);
            if (cs2 != null) total.AddRange(cs2);
            return new string(total.ToArray());
        }
        /// <summary>
        /// 将多个字符数组合并转换为string对象
        /// </summary>
        /// <param name="cs1">字符数组1</param>
        /// <param name="cs2">字符数组2</param>
        /// <param name="cs3">字符数组3</param>
        public static string AsString(List<char> cs1, List<char> cs2, List<char> cs3)
        {
            if (cs1 == null && cs2 == null && cs3 == null)
                return null;

            List<char> total = new List<char>();
            if (cs1 != null) total.AddRange(cs1);
            if (cs2 != null) total.AddRange(cs2);
            if (cs3 != null) total.AddRange(cs3);
            return new string(total.ToArray());
        }
        /// <summary>
        /// 将多个字符数组合并转换为string对象
        /// </summary>
        /// <param name="cs1">字符数组1</param>
        /// <param name="cs2">字符数组2</param>
        /// <param name="cs3">字符数组3</param>
        /// <param name="cs4">字符数组4</param>
        public static string AsString(List<char> cs1, List<char> cs2, List<char> cs3, List<char> cs4)
        {
            if (cs1 == null && cs2 == null && cs3 == null && cs4 == null)
                return null;

            List<char> total = new List<char>();
            if (cs1 != null) total.AddRange(cs1);
            if (cs2 != null) total.AddRange(cs2);
            if (cs3 != null) total.AddRange(cs3);
            if (cs4 != null) total.AddRange(cs4);
            return new string(total.ToArray());
        }
        /// <summary>
        /// 将多个字符数组合并转换为string对象
        /// </summary>
        /// <param name="cs1">字符数组1</param>
        /// <param name="cs2">字符数组2</param>
        /// <param name="cs3">字符数组3</param>
        /// <param name="cs4">字符数组4</param>
        /// <param name="cs5">字符数组5</param>
        public static string AsString(List<char> cs1, List<char> cs2, List<char> cs3, List<char> cs4, List<char> cs5)
        {
            if (cs1 == null && cs2 == null && cs3 == null && cs4 == null && cs5 == null)
                return null;

            List<char> total = new List<char>();
            if (cs1 != null) total.AddRange(cs1);
            if (cs2 != null) total.AddRange(cs2);
            if (cs3 != null) total.AddRange(cs3);
            if (cs4 != null) total.AddRange(cs4);
            if (cs5 != null) total.AddRange(cs5);
            return new string(total.ToArray());
        }
        /// <summary>
        /// 字符数组是否由指定的字符组成，且至少包含count个字符
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="c">指定的字符</param>
        /// <param name="count">指定的字符数量</param>
        public static bool IsComposedOf(this List<char> cs, char c, int count)
        {
            if (cs.Count < count)
                return false;

            for (int i = 0; i < cs.Count; i++)
            {
                if (cs[i] != c)
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 是否等于指定字符串
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="str">指定字符串</param>
        public static bool Equal(this List<char> cs, string str)
        {
            if (cs == null || string.IsNullOrEmpty(str))
                return false;

            if (cs.Count != str.Length)
                return false;

            for (int i = 0; i < cs.Count; i++)
            {
                if (str[i] != cs[i])
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 从startIndex开始，查找数组中目标字符c的第一个索引，如果没有则返回-1
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="c">目标字符</param>
        /// <param name="startIndex">开始索引</param>
        public static int IndexOfChar(this List<char> cs, char c, int startIndex)
        {
            for (int i = startIndex; i < cs.Count; i++)
            {
                if (cs[i] == c)
                {
                    return i;
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取一个子级List
        /// </summary>
        /// <param name="cs">字符数组</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="length">长度</param>
        public static List<char> SubList(this List<char> cs, int startIndex, int length)
        {
            List<char> sub = new List<char>();
            int index = startIndex;
            for (int i = 0; i < length; i++)
            {
                if (index < cs.Count)
                {
                    sub.Add(cs[index]);
                    index += 1;
                }
            }
            return sub;
        }
        /// <summary>
        /// 将字符串转换为List字符数组
        /// </summary>
        /// <param name="str">字符数组</param>
        public static List<char> AsCharList(this string str)
        {
            List<char> cs = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                cs.Add(str[i]);
            }
            return cs;
        }
        /// <summary>
        /// 从 Assembly-CSharp-Editor 程序集中获取所有类型
        /// </summary>
        /// <returns>所有类型</returns>
        public static List<Type> GetTypesInEditor()
        {
            List<Type> types = new List<Type>();
            Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblys.Length; i++)
            {
                string name = assemblys[i].GetName().Name;
                if (name == "Assembly-CSharp-Editor")
                {
                    types.AddRange(assemblys[i].GetTypes());
                    break;
                }
            }
            return types;
        }
        /// <summary>
        /// 是否为合格的自定义块回调方法
        /// </summary>
        /// <param name="methodInfo">方法</param>
        public static bool IsBlockCustomActionMethod(this MethodInfo methodInfo)
        {
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (parameters != null && parameters.Length == 1 && parameters[0].ParameterType == typeof(string))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 解析文档
        /// </summary>
        /// <param name="assetPath">文档资源路径</param>
        /// <param name="content">文档内容</param>
        /// <param name="isHideLoadFailImage">是否隐藏加载失败的图像</param>
        /// <returns>文档对象</returns>
        public static MarkdownDocument Parse(string assetPath, string content, bool isHideLoadFailImage)
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
        private static MarkdownContainer ParseLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return null;

            List<char> lineChars = line.AsCharList();
            MarkdownContainer container = new MarkdownContainer(ParseContainerType(lineChars));
            List<char> keywordsStart = new List<char>();
            List<char> text = new List<char>();
            List<char> keywordsEnd = new List<char>();
            for (int i = 0; i < lineChars.Count; i++)
            {
                char c = lineChars[i];
                if (c == '*' || c == '_' || c == '[' || c == ']' || c == '{' || c == '}' || c == '!' || c == '`')
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
                        //对象、自定义
                        else if (keywordsStart.StartWith('{') && keywordsEnd.EndWith('}'))
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
        private static MarkdownContainer ParseFragment(string fragment)
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
        /// <param name="args1">附带参数1</param>
        /// <param name="args2">附带参数2</param>
        private static MarkdownBlock ParseBlock(List<char> keywordsStart, List<char> text, List<char> keywordsEnd, List<char> args1 = null, List<char> args2 = null)
        {
            if (args1 != null && args2 != null)
            {
                if (keywordsStart.StartWith("[![") && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveRange(0, 3);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    args1.RemoveAt(0);
                    args1.RemoveAt(args1.Count - 1);
                    args2.RemoveAt(0);
                    args2.RemoveAt(args2.Count - 1);
                    return new BlockImageLink(AsString(keywordsStart, text, keywordsEnd), args1.AsString(), args2.AsString());
                }
            }
            else if (args1 != null)
            {
                if (keywordsStart.StartWith("![") && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    args1.RemoveAt(0);
                    args1.RemoveAt(args1.Count - 1);
                    return new BlockImage(AsString(keywordsStart, text, keywordsEnd), args1.AsString());
                }
                if (keywordsStart.StartWith('[') && keywordsEnd.EndWith(']'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    args1.RemoveAt(0);
                    args1.RemoveAt(args1.Count - 1);
                    return new BlockLink(AsString(keywordsStart, text, keywordsEnd), args1.AsString());
                }
                if (keywordsStart.StartWith('{') && text.Equal("Object") && keywordsEnd.EndWith('}'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    args1.RemoveAt(0);
                    args1.RemoveAt(args1.Count - 1);
                    return new BlockObject(AsString(keywordsStart, text, keywordsEnd), args1.AsString());
                }
                if (keywordsStart.StartWith('{') && text.Equal("Custom") && keywordsEnd.EndWith('}'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    args1.RemoveAt(0);
                    args1.RemoveAt(args1.Count - 1);
                    return new BlockCustom(AsString(keywordsStart, text, keywordsEnd), args1.AsString());
                }
            }
            else
            {
                if (keywordsStart.StartWith("**") && keywordsEnd.EndWith("**"))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveRange(keywordsEnd.Count - 2, 2);
                    return new BlockAsteriskTwo(AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith('*') && keywordsEnd.EndWith('*'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    return new BlockAsterisk(AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith("__") && keywordsEnd.EndWith("__"))
                {
                    keywordsStart.RemoveRange(0, 2);
                    keywordsEnd.RemoveRange(keywordsEnd.Count - 2, 2);
                    return new BlockUnderlineTwo(AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith('_') && keywordsEnd.EndWith('_'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    return new BlockUnderline(AsString(keywordsStart, text, keywordsEnd));
                }
                if (keywordsStart.StartWith('`') && keywordsEnd.EndWith('`'))
                {
                    keywordsStart.RemoveAt(0);
                    keywordsEnd.RemoveAt(keywordsEnd.Count - 1);
                    return new BlockCode(AsString(keywordsStart, text, keywordsEnd));
                }
            }
            return new BlockDefault(AsString(keywordsStart, text, keywordsEnd, args1, args2));
        }
        /// <summary>
        /// 解析行的容器类型
        /// </summary>
        /// <param name="lineChars">行内容</param>
        private static ContainerType ParseContainerType(List<char> lineChars)
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
            else if (lineChars.IsComposedOf('*', 3) || lineChars.IsComposedOf('-', 3) || lineChars.IsComposedOf('_', 3))
            {
                type = ContainerType.DividingLine;
                lineChars.Clear();
            }
            lineChars.Trim();
            return type;
        }
    }
}