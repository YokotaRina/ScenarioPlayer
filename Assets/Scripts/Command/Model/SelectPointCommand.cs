using Command.Enum;

namespace Command.Model
{
    /// <summary>
    /// 選択肢の分岐地点コマンドの管理クラス
    /// </summary>
    public class SelectPointCommand : CommandBase
    {
        /// <summary>
        /// 分岐地点ID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectPointCommand(AdvCommandType advCommandType, string id) : base(advCommandType)
        {
            _id = id;
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