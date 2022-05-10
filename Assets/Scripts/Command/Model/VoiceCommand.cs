using Command.Enum;

namespace Command.Model
{
    /// <summary>
    /// ボイス再生コマンドの管理クラス
    /// </summary>
    public class VoiceCommand : CommandBase
    {
        /// <summary>
        /// 再生ボイスID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VoiceCommand(AdvCommandType advCommandType, string id) : base(advCommandType)
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
