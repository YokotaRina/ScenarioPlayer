using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Ruby
{
    /// <summary>
    /// タグの解析用クラス
    /// </summary>
    public class TagAnalyzer : MonoBehaviour
    {
        // ルビタグ名
        const string TEXT_TAG_RUBY = "ruby";

        // タグ情報の抽出インデックス
        const int TAG_NAME_GROUP = 2;
        const int TAG_VALUE_GROUP = 3;
        const int VALUE_GROUP = 4;
        const int NO_TAG_GROUP = 5;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            // テスト
            GetRubySettingList("<ruby=ルビだよ>ここにルビ</ruby>を振ります。");
        }

        /// <summary>
        /// ルビ設定用のリストを作成する
        /// </summary>
        public List<RubySetting> GetRubySettingList(string text)
        {
            // タグとそれ以外でグループを分けてマッチさせる
            // ※拡張性を持たせるため、ルビタグ以外もマッチできるようにしておく
            var matchCollection = Regex.Matches(text, @"(<(.*?)(=.*?)?>([\s\S]*?)<\/\2>|([^<]+))");

            var settingList = new List<RubySetting>();

            foreach (Match match in matchCollection)
            {
                var group = match.Groups;

                // タグ内容（例：ruby=hoge）
                var tag = group[TAG_NAME_GROUP].Value + group[TAG_VALUE_GROUP].Value;
                // タグにはさまれた文字
                var targetText = group[VALUE_GROUP].Value;

                // ルビの設定
                if (tag.Contains(TEXT_TAG_RUBY))
                {
                    // ルビに不要な文字列を消す
                    // TODO:開始タグと終了タグの設定
                    var rubyText = tag.Replace("ruby=", "");
                    var rubySetting = new RubySetting(targetText, rubyText);
                    settingList.Add(rubySetting);
                }
            }

            return settingList;
        }
    }
}
