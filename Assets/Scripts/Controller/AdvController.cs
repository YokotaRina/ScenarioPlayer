using System.Collections.Generic;
using Model.Character;
using Model.Command;
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
        /// 初期化
        /// </summary>
        public void Initialize(string fileName)
        {
            _commandList = new CommandRepository().GetCommandList(fileName);

            var characterRepository = new CharacterRepository();
            _characterList = characterRepository.GetCharacterList();
            _positionList = characterRepository.GetPositionList();
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
