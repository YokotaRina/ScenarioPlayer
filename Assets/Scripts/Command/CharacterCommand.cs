using Controller;
using Enums;

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
        /// 表情パターン
        /// </summary>
        private readonly FacePattern _facePattern;
        public FacePattern FacePattern => _facePattern;

        /// <summary>
        /// 表示位置ID
        /// </summary>
        private readonly string _positionId;
        public string PositionId => _positionId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CharacterCommand(AdvCommandType advCommandType, string id, FacePattern facePattern, string positionId) : base(advCommandType)
        {
            _id = id;
            _facePattern = facePattern;
            _positionId = positionId;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public override void Start(AdvController controller)
        {
            controller.DisplayCharacter(this);
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