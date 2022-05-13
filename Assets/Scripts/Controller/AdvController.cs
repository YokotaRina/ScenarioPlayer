using System.Collections.Generic;
using Command;
using Model.Character;
using Model.Sound;
using Model.Texture;
using Repository;
using UnityEngine;

namespace Controller
{
    /// <summary>
    /// Advのコントローラー
    /// </summary>
    public class AdvController : MonoBehaviour
    {
        /// <summary>
        /// コマンドリスト
        /// </summary>
        private List<CommandBase> _commandList;

        /// <summary>
        /// キャラクターリスト
        /// </summary>
        private List<CharacterBase> _characterList;

        /// <summary>
        /// キャラクター表示位置リスト
        /// </summary>
        private List<PositionBase> _positionList;

        /// <summary>
        /// 背景情報リスト
        /// </summary>
        private List<BackGroundBase> _backGroundList;

        /// <summary>
        /// ボイス情報リスト
        /// </summary>
        private List<VoiceBase> _voiceList;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize(string fileName)
        {
            _commandList = new CommandRepository().GetCommandList(fileName);

            var characterRepository = new CharacterRepository();
            _characterList = characterRepository.GetCharacterList();
            _positionList = characterRepository.GetPositionList();

            var resourceRepository = new ResourceRepository();
            _backGroundList = resourceRepository.GetBackGroundList();
            _voiceList = resourceRepository.GetVoiceList();
        }

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            if (_commandList != null)
            {
                _commandList.Clear();
                _commandList = null;
            }

            if (_characterList != null)
            {
                _characterList.Clear();
                _characterList = null;
            }

            if (_positionList != null)
            {
                _positionList.Clear();
                _positionList = null;
            }
        }
    }
}
