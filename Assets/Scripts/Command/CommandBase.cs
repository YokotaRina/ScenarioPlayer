using Controller;
using Enums;

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
        private readonly AdvCommandType _advCommandType;
        public AdvCommandType AdvCommandType => _advCommandType;

        /// <summary>
        /// コマンドが終了したか？
        /// </summary>
        private bool _isEnd;
        public bool IsEnd => _isEnd;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        protected CommandBase(AdvCommandType advCommandType)
        {
            _advCommandType = advCommandType;
            _isEnd = false;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public abstract void Start(AdvController controller);

        /// <summary>
        /// 終了
        /// </summary>
        public abstract void End();

        protected void SetEnd()
        {
            _isEnd = true;
        }
    }
}
