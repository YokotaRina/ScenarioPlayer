using Controller;
using Enums;
using Ruby;

namespace Command
{
    /// <summary>
    /// テキスト表示コマンドの管理クラス
    /// </summary>
    public class TextCommand : CommandBase
    {
        /// <summary>
        /// 表示テキスト
        /// </summary>
        private string _text;
        public string Text => _text;

        /// <summary>
        /// 表示キャラクター名
        /// </summary>
        private readonly string _name;
        public string Name => _name;

        /// <summary>
        /// テキストサイズ
        /// ※0の場合はデフォルト
        /// </summary>
        private readonly int _size;
        public int Size => _size;

        /// <summary>
        /// テキストカラー
        /// ※空の場合はデフォルト
        /// </summary>
        private readonly string _color;
        public string Color => _color;

        // テキスト表示用のフォーマット
        private const string CHANGE_SIZE_TEXT_FORMAT = "<size={0}>{1}</size>";
        private const string CHANGE_COLOR_TEXT_FORMAT = "<color={0}>{1}</color>";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TextCommand(AdvCommandType advCommandType, string text, string name, int size, string color) : base(advCommandType)
        {
            _text = text;
            _name = name;
            _size = size;
            _color = color;
        }

        /// <summary>
        /// 表示テキスト取得
        /// </summary>
        /// <returns></returns>
        public string GetMessageText()
        {
            if (_size != 0)
            {
                _text = string.Format(CHANGE_SIZE_TEXT_FORMAT, _size, _text);
            }

            if (!string.IsNullOrEmpty(_color))
            {
                _text = string.Format(CHANGE_COLOR_TEXT_FORMAT, _color, _text);
            }

            return _text;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public override void Start(AdvController controller)
        {
            // ルビの解析 → タグ置き換え
            var rubySettingList = TagAnalyzer.GetRubySettingList(_text);
            if (rubySettingList.Count > 0)
            {
                foreach (var rubySetting in rubySettingList)
                {
                    controller.RubyTagGenerator.MakeRubyTag(rubySetting);
                    var afterText = rubySetting.GetRubyText();
                    _text = _text.Replace($"<ruby={rubySetting.RubyText}>{rubySetting.TargetText}</ruby>", afterText);
                }
            }

            controller.UpdateMessageText(this);
        }

        /// <summary>
        /// 終了
        /// </summary>
        public override void End()
        {
            base.SetEnd();
        }
    }
}
