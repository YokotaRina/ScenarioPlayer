namespace Model.Sound
{
    /// <summary>
    /// ボイス情報保持クラス
    /// </summary>
    public class VoiceBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// リソース
        /// </summary>
        private readonly string _resource;
        public string Resource => _resource;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VoiceBase(string id, string resource)
        {
            _id = id;
            _resource = resource;
        }
    }
}
