using Utils;

namespace Enums
{
    /// <summary>
    /// 表情の種類
    /// </summary>
    public enum FacePattern
    {
        [Display(Name = "指定なし")]
        None = 0,

        [Display(Name = "通常")]
        Normal = 1,

        [Display(Name = "笑顔")]
        Smile = 2,

        [Display(Name = "怒り")]
        Angry = 3,

        [Display(Name = "悲しみ")]
        Sad = 4,
    }
}
