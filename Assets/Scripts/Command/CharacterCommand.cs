using Command.Enum;

namespace Command
{
    /// <summary>
    /// キャラクター表示コマンドの管理クラス
    /// </summary>
    public class CharacterCommand : CommandBase
    {
        /// <summary>
        /// 表示キャラクターID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// 表情パターンID
        /// </summary>
        private readonly string _patternId;
        public string PatternId => _patternId;

        /// <summary>
        /// 表示位置ID
        /// </summary>
        private readonly int _positionId;
        public int PositionId => _positionId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterCommand(CommandType commandType, string id, string patternId, int positionId) : base(commandType)
        {
            _id = id;
            _patternId = patternId;
            _positionId = positionId;
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