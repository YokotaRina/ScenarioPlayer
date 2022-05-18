namespace Model.Sound
{
    /// <summary>
    /// BGM情報保持クラス
    /// </summary>
    public class BgmBase
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
        /// ループさせるか
        /// </summary>
        private readonly bool _isLoop;
        public bool IsLoop => _isLoop;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BgmBase(string id, string resource, bool isLoop)
        {
            _id = id;
            _resource = resource;
            _isLoop = isLoop;
        }
    }
}
