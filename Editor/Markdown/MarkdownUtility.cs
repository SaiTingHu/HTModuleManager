using System.Collections.Generic;

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
        /// 将字符串转换为List<char>
        /// </summary>
        /// <param name="str">字符数组</param>
        public static List<char> AsList(this string str)
        {
            List<char> cs = new List<char>();
            for (int i = 0; i < str.Length; i++)
            {
                cs.Add(str[i]);
            }
            return cs;
        }
    }
}