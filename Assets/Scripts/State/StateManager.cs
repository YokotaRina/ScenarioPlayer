namespace Script.State
{
    /// <summary>
    /// Stateの管理クラス
    /// </summary>
    public class StateManager
    {
        /// <summary>
        /// 使用していない状態を示す値
        /// </summary>
        private static int _unusedState = 99999;

        /// <summary>
        /// Stateの情報
        /// </summary>
        private StateInfo _stateInfo = new StateInfo();
        public StateInfo StateInfo => _stateInfo;

        /// <summary>
        /// 子ステート
        /// </summary>
        private StateManager _childState;

        /// <summary>
        /// 現在のステート
        /// </summary>
        private int _currentState;

        /// <summary>
        /// 前のステート
        /// </summary>
        private int _prevState;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _currentState = 0;
            _prevState = 0;
            _stateInfo.Initialize();

            // 子ステートが有効な場合
            if (_childState != null) _childState.Initialize();
        }

        /// <summary>
        /// ステートの設定
        /// </summary>
        public void SetState(int state, bool isLoop = true)
        {
            _prevState = _currentState;
            _currentState = state;
            _stateInfo.Initialize();

            // ループ設定が有効な場合
            if (isLoop) _stateInfo.AddFlag(StateInfo.Flag.IsLoop);

            // 子ステートが有効な場合
            if (_childState != null) _childState.Initialize();
        }

        /// <summary>
        /// 現在のステートの取得
        /// </summary>
        /// <returns></returns>
        public int GetState()
        {
            return _currentState;
        }

        /// <summary>
        /// 指定のステートか
        /// </summary>
        public bool IsState(int state)
        {
            return _currentState == state;
        }

        /// <summary>
        /// ステートの終了
        /// </summary>
        public void End()
        {
            this.SetState(_unusedState, false);
        }

        /// <summary>
        /// 終了しているか
        /// </summary>
        public bool IsEnd()
        {
            return this.IsState(_unusedState);
        }

        /// <summary>
        /// 前のステートへ
        /// </summary>
        public void Prev(bool isLoop = true)
        {
            this.SetState(_currentState - 1, isLoop);
        }

        /// <summary>
        /// 次のステートへ
        /// </summary>
        public void Next(bool isLoop = true)
        {
            this.SetState(_currentState + 1, isLoop);
        }

        /// <summary>
        /// 子ステート取得
        /// </summary>
        public StateManager GetChild()
        {
            return _childState != null ? _childState : _childState = new StateManager();
        }

        /// <summary>
        /// 初回進入時か
        /// </summary>
        public bool IsOnEntry()
        {
            return _stateInfo.OneTimeFlag(StateInfo.Flag.OnEntry);
        }

        /// <summary>
        /// ループ中か
        /// </summary>
        public bool IsLoop()
        {
            return _childState != null
                ? _childState.IsLoop() || _stateInfo.OneTimeFlag(StateInfo.Flag.IsLoop)
                : _stateInfo.OneTimeFlag(StateInfo.Flag.IsLoop);
        }

        /// <summary>
        /// 指定した時間が経過したか
        /// </summary>
        public bool WaitTime(float duration)
        {
            return _stateInfo.Time > duration;
        }

        /// <summary>
        /// Update
        /// </summary>
        public void Update(float addTime)
        {
            if (_currentState == _prevState)
            {
                _stateInfo.Update(addTime);

                // 子ステートが有効な場合
                if (_childState != null) _childState.Update(addTime);
            }

            _prevState = _currentState;
        }
    }
}
