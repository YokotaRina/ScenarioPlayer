namespace Ruby
{
    /// <summary>
    /// ルビ設定用クラス
    /// </summary>
    public class RubySetting
    {
        /// <summary>
        /// ルビを振りたいテキスト
        /// </summary>
        private string _targetText;
        public string TargetText => _targetText;

        /// <summary>
        /// ルビ用のテキスト
        /// </summary>
        private string _rubyText;
        public string RubyText => _rubyText;

        /// <summary>
        /// ルビ開始のタグ
        /// </summary>
        private string _rubyStartTag;
        public string RubyStartTag => _rubyStartTag;

        /// <summary>
        /// ルビ終了のタグ
        /// </summary>
        private string _rubyEndTag;
        public string RubyEndTag => _rubyEndTag;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RubySetting(string targetText, string rubyText)
        {
            _targetText = targetText;
            _rubyText = rubyText;
        }
    }
}
