using Controller;
using Enums;

namespace Command
{
    /// <summary>
    /// ジャンプ先地点コマンドの管理クラス
    /// </summary>
    public class JumpPointCommand : CommandBase
    {
        /// <summary>
        /// ジャンプ先地点ID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JumpPointCommand(AdvCommandType advCommandType, string id) : base(advCommandType)
        {
            _id = id;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public override void Start(AdvController controller)
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