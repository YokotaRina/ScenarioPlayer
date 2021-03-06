using System.Collections.Generic;
using Enums;

namespace Model.Character
{
    /// <summary>
    /// キャラクター情報保持クラス
    /// </summary>
    public class CharacterBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// キャラクター名
        /// </summary>
        private readonly string _name;
        public string Name => _name;

        /// <summary>
        /// 表情とリソースの紐付けDictionary
        /// </summary>
        private readonly Dictionary<FacePattern, string> _resourceDictionary;
        public Dictionary<FacePattern, string>  ResourceDictionary => _resourceDictionary;

        /// <summary>
        /// 表示スケール
        /// </summary>
        private readonly int _scale;
        public int Scale => _scale;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterBase(string id, string name, Dictionary<FacePattern, string> resourceDictionary, int scale)
        {
            _id = id;
            _name = name;
            _resourceDictionary = resourceDictionary;
            _scale = scale;
        }
    }
}
