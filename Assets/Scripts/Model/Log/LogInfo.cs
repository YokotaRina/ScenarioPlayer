namespace Model.Log
{
    /// <summary>
    /// ログ情報を保持するモデル
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// 名前
        /// ※空の場合は名前なし
        /// </summary>
        private readonly string _name;
        public string Name => _name;

        /// <summary>
        /// テキスト
        /// </summary>
        private readonly string _text;
        public string Text => _text;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LogInfo(string name, string text)
        {
            _name = name;
            _text = text;
        }
    }
}
