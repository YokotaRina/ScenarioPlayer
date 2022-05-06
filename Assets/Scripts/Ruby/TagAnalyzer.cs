using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ruby
{
    /// <summary>
    /// タグの解析用クラス
    /// </summary>
    public class TagAnalyzer
    {
        // ルビタグ名
        const string TEXT_TAG_RUBY = "ruby";

        // タグ情報の抽出インデックス
        const int TAG_NAME_GROUP = 2;
        const int TAG_VALUE_GROUP = 3;
        const int VALUE_GROUP = 4;
        const int NO_TAG_GROUP = 5;

        /// <summary>
        /// ルビ設定用のリストを作成する
        /// </summary>
        public static List<RubySetting> GetRubySettingList(string text)
        {
            var settingList = new List<RubySetting>();
            if (!text.Contains(TEXT_TAG_RUBY)) return settingList;

            // タグとそれ以外でグループを分けてマッチさせる
            // ※拡張性を持たせるため、ルビタグ以外もマッチできるようにしておく
            var matchCollection = Regex.Matches(text, @"(<(.*?)(=.*?)?>([\s\S]*?)<\/\2>|([^<]+))");

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
                    var rubyText = tag.Replace($"{TEXT_TAG_RUBY}=", "");
                    var rubySetting = new RubySetting(targetText, rubyText);
                    settingList.Add(rubySetting);
                }
            }

            return settingList;
        }
    }
}
