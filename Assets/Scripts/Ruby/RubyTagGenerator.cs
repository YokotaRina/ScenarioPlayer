using TMPro;
using UnityEngine;

namespace Ruby
{
    /// <summary>
    /// ルビ表示用タグの生成クラス
    /// </summary>
    public class RubyTagGenerator : MonoBehaviour
    {
        [SerializeField, Tooltip("表示テキスト")]
        private TextMeshProUGUI text = default;

        /// <summary>
        /// ルビのサイズ(%)
        /// </summary>
        private float _rubySize = 40f;

        /// <summary>
        /// ルビ用テキストの上部へずらす量設定用開始タグ
        /// </summary>
        private string _rubyVOffsetStartTag => $"<voffset=1em>";

        /// <summary>
        /// ルビ用テキストの上部へずらす量設定用終了タグ
        /// </summary>
        private string _rubyVOffsetEndTag => "</voffset>";

        /// <summary>
        /// ルビ用テキストのサイズ設定用開始タグ
        /// </summary>
        private string _rubySizeStartTag => $"<size={_rubySize:0.####}%>";

        /// <summary>
        /// ルビ用テキストのサイズ設定用終了タグ
        /// </summary>
        private string _rubySizeEndTag = "</size>";

        /// <summary>
        /// ルビ表示用のタグを生成
        /// </summary>
        private void MakeRubyTag(RubySetting rubySetting)
        {
            var rubyText = rubySetting.RubyText;
            if (string.IsNullOrEmpty(rubyText)) return;

            // ルビを振りたいテキストの中心への移動量を取得
            // （文字数 + 文字数分の文字間隔） / 2）
            var textLength = rubySetting.TargetText.Length;
            var textSpace = GetCharacterSpacingWidth(textLength);
            var textCenter = (textLength + textSpace) / 2f;

            // ルビ用テキストの中心への移動量を取得
            // （（（文字数 * 文字サイズの割合）+ 文字数分の文字間隔） / 2）
            var rubyTextLength = rubyText.Length;
            var rubyTextSpace = GetCharacterSpacingWidth(rubyText.Length);
            var rubyCenter = (rubyTextLength * (_rubySize / 100f) + rubyTextSpace) / 2f;

            // ルビ用テキストの開始位置
            var rubyPosition = (textCenter + rubyCenter) * -1;
            var rubyPositionTag = $"<space={rubyPosition:0.####}em>";

            // ルビの後の文字の開始位置
            var nextTextPosition = (rubyCenter - textCenter) * -1;
            var nextTextPositionTag = $"<space={nextTextPosition:0.####}em>";

            // 開始タグ
            var rubyStartTag = $"{rubyPositionTag}{_rubyVOffsetStartTag}{_rubySizeStartTag}";
            // 終了タグ
            var rubyEndTag = $"{_rubySizeEndTag}{_rubyVOffsetEndTag}{nextTextPositionTag}";

            // タグの設定
            rubySetting.SetTag(rubyStartTag, rubyEndTag);
        }

        /// <summary>
        /// 文字数分の間隔を取得する
        /// </summary>
        private float GetCharacterSpacingWidth(int textLength)
        {
            return textLength * (text.characterSpacing / 100f);
        }
    }
}
