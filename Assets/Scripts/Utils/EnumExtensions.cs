using System;

namespace Utils
{
    /// <summary>
    /// Enum拡張クラス
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 表示用文字列を返す
        /// </summary>
        public static string ToDisplayName(this Enum enumInstance)
        {
            var type = enumInstance.GetType();
            var fieldInfo = type.GetField(enumInstance.ToString());
            var descriptionAttributes = fieldInfo.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
            
            if (descriptionAttributes != null && descriptionAttributes.Length > 0)
            {
                return descriptionAttributes[0].Name;
            }

            return enumInstance.ToString();
        }
    }
}
