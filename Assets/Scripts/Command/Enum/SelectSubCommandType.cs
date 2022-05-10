using Utils;

namespace Command.Enum
{
    /// <summary>
    /// 選択肢のサブコマンドの種類
    /// </summary>
    public enum SelectSubCommandType
    {
        [Display(Name = "指定なし")]
        None = 0,

        [Display(Name = "選択肢テキスト")]
        ChoiceWord = 1,

        [Display(Name = "テキスト表示")]
        Text = 2,

        [Display(Name = "キャラクター表示")]
        Character = 3,

        [Display(Name = "ボイス再生")]
        Voice = 4,

        [Display(Name = "エフェクト再生")]
        Effect = 5,
    }
}
