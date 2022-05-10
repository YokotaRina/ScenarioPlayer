using Command.Enum;

namespace Command.Model
{
    /// <summary>
    /// コマンドのベースクラス
    /// ※各コマンドのクラスにはこのクラスを継承すること
    /// </summary>
    public abstract class CommandBase
    {
        /// <summary>
        /// コマンドの種類
        /// </summary>
        private readonly AdvCommandType _advCommandType;
        public AdvCommandType AdvCommandType => _advCommandType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CommandBase(AdvCommandType advCommandType)
        {
            _advCommandType = advCommandType;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// 終了
        /// </summary>
        public abstract void End();
    }
}
