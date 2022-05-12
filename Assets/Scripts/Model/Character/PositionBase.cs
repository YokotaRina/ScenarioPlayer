namespace Model.Character
{
    /// <summary>
    /// キャラクターの表示位置情報保持クラス
    /// </summary>
    public class PositionBase
    {
        /// <summary>
        /// ID
        /// </summary>
        private readonly string _id;
        public string Id => _id;

        /// <summary>
        /// X座標
        /// </summary>
        private readonly int _x;
        public int X => _x;
        
        /// <summary>
        /// Y座標
        /// </summary>
        private readonly int _y;
        public int Y => _y;

        /// <summary>
        /// 描画順
        /// </summary>
        private readonly int _order;
        public int Order => _order;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PositionBase(string id, int x, int y, int order)
        {
            _id = id;
            _x = x;
            _y = y;
            _order = order;
        }
    }
}
