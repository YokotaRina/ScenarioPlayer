using Command.Enum;
using JetBrains.Annotations;

namespace Command.Model
{
    /// <summary>
    /// テキスト表示コマンドの管理クラス
    /// </summary>
    public class TextCommand : CommandBase
    {
        /// <summary>
        /// 表示テキスト
        /// </summary>
        private readonly string _text;
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
        /// 開始
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// 終了
        /// </summary>
        public override void End()
        {
        }
    }
}