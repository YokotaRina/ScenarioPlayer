using Controller;
using Enums;

namespace Command
{
    /// <summary>
    /// BGM再生コマンドの管理クラス
    /// </summary>
    public class BgmCommand : CommandBase
    {
        /// <summary>
        /// 再生BGMID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public BgmCommand(AdvCommandType advCommandType, string id) : base(advCommandType)
        {
            _id = id;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public override void Start(AdvController controller)
        {
            controller.PlayBgm(this);
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
