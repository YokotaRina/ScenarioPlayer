using Enums;

namespace Model.Command
{
    /// <summary>
    /// 背景表示コマンドの管理クラス
    /// </summary>
    public class BackGroundCommand : CommandBase
    {
        /// <summary>
        /// 表示背景D
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BackGroundCommand(AdvCommandType advCommandType, string id) : base(advCommandType)
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
