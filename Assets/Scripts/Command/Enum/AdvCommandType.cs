using Utils;

namespace Command.Enum
{
    /// <summary>
    /// コマンドの種類
    /// </summary>
    public enum AdvCommandType
    {
        [Display(Name = "指定なし")]
        None = 0,

        [Display(Name = "テキスト表示")]
        Text = 1,

        [Display(Name = "キャラクター表示")]
        Character = 2,

        [Display(Name = "背景表示")]
        BackGround = 3,

        [Display(Name = "ボイス再生")]
        Voice = 4,

        [Display(Name = "BGM再生")]
        BGM = 5,

        [Display(Name = "選択肢")]
        Select = 6,

        [Display(Name = "選択肢終了")]
        SelectEnd = 7,

        [Display(Name = "選択肢の分岐地点")]
        SelectPoint = 8,

        [Display(Name = "ジャンプ")]
        Jump = 9,

        [Display(Name = "ジャンプ先地点")]
        JumpPoint = 10,

        [Display(Name = "コメント")]
        Comment = 99,
    }
}
