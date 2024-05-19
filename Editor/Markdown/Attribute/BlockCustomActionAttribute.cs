using System;

namespace HT.ModuleManager.Markdown
{
    /// <summary>
    /// Markdown自定义块的回调方法，仅可标记静态函数
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BlockCustomActionAttribute : Attribute
    {
        /// <summary>
        /// Markdown自定义块的回调方法，仅可标记静态函数
        /// </summary>
        public BlockCustomActionAttribute()
        { 
        
        }
    }
}