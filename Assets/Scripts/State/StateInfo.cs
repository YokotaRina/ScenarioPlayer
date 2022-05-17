using System;

namespace Script.State
{
    /// <summary>
    /// Stateの情報クラス
    /// </summary>
    public class StateInfo
    {
        /// <summary>
        /// ステートの状態を示すビットフラグ
        /// </summary>
        [Flags]
        public enum Flag
        {
            None = 0,         // 何もしていない
            OnEntry = 1 << 0, // ステート進入
            IsLoop = 1 << 1,  // ループ中
        }

        /// <summary>
        /// 現在のステートの状態を示すビットフラグ
        /// </summary>
        private Flag _flag { get; set; }

        /// <summary>
        /// 経過時間
        /// </summary>
        public float Time { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StateInfo()
        {
            this.Initialize();
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            AddFlag(Flag.OnEntry);
            Time = 0;
        }

        /// <summary>
        /// ビットフラグを追加する
        /// </summary>
        public void AddFlag(Flag flag)
        {
            _flag |= flag;
        }

        /// <summary>
        /// ビットフラグを削除する
        /// </summary>
        public void DeleteFlag(Flag flag)
        {
            _flag &= ~flag;
        }

        /// <summary>
        /// ビットフラグを全て削除する
        /// </summary>
        public void DeleteAllFlag()
        {
            _flag = Flag.None;
        }

        /// <summary>
        /// 指定のビットフラグを持つか
        /// </summary>
        public bool HasFlag(Flag flag)
        {
            return _flag.HasFlag(flag);
        }

        /// <summary>
        /// 初回の1度のみ判定する
        /// </summary>
        public bool OneTimeFlag(Flag flag)
        {
            var result = this.HasFlag(flag);
            this.DeleteFlag(flag);
            return result;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update(float addTime)
        {
            this.DeleteAllFlag();
            Time += addTime;
        }
    }
}
