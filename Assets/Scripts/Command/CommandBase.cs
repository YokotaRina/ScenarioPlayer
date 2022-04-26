using Command.Enum;

namespace Command
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
        private readonly CommandType _commandType;
        public CommandType CommandType => _commandType;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CommandBase(CommandType commandType)
        {
            _commandType = commandType;
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
