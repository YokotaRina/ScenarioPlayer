using System;
using System.Collections.Generic;
using Enums;

namespace Model.Command
{
    /// <summary>
    /// 選択肢コマンドの管理クラス
    /// </summary>
    public class SelectCommand : CommandBase
    {
        /// <summary>
        /// 選択肢のリスト
        /// </summary>
        private readonly List<Tuple<string/*choiceWord*/, string>/*SelectPointId*/> _choiceWordList;
        public List<Tuple<string, string>> ChoiceWordList => _choiceWordList;
 
        /// <summary>
        /// 表示テキスト
        /// ※空の場合は非表示
        /// </summary>
        private readonly string _text;
        public string Text => _text;

        /// <summary>
        /// 表示キャラクターID
        /// ※空の場合は非表示
        /// </summary>
        private readonly string _characterId;
        public string CharacterId => _characterId;

        /// <summary>
        /// 再生ボイスID
        /// ※空の場合は何も再生しない
        /// </summary>
        private readonly string _voiceId;
        public string VoiceId => _voiceId;

        /// <summary>
        /// 再生エフェクトID
        /// ※空の場合は何も再生しない
        /// </summary>
        private readonly string _effectId;
        public string EffectId => _effectId;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SelectCommand(
            AdvCommandType advCommandType,
            List<Tuple<string, string>> choiceWordList,
            string text,
            string characterId,
            string voiceId,
            string effectId
        ) : base(advCommandType)
        {
            _choiceWordList = choiceWordList;
            _text = text;
            _characterId = characterId;
            _voiceId = voiceId;
            _effectId = effectId;
        }

        /// <summary>
        /// 開始
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// 終了
        /// </summary>
        public override void End()
        {
        }
    }
}
