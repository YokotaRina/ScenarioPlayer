using System;

namespace Utils
{
    /// <summary>
    /// Enum表示用の文字列を設定するAttribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DisplayAttribute : Attribute
    {
        /// <summary>
        /// 表示名
        /// </summary>
        public string Name { get; set; }
    }
}
