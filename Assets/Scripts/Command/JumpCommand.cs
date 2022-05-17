using Controller;
using Enums;

namespace Command
{
    /// <summary>
    /// ジャンプコマンドの管理クラス
    /// </summary>
    public class JumpCommand : CommandBase
    {
        /// <summary>
        /// ジャンプ地点のID
        /// </summary>
        private readonly string _jumpPointId;
        public string JumpPointId => _jumpPointId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public JumpCommand(AdvCommandType advCommandType, string jumpPointId) : base(advCommandType)
        {
            _jumpPointId = jumpPointId;
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
