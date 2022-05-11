using System.Collections.Generic;
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
        /// 初期化
        /// </summary>
        public void Initialize(string fileName)
        {
            _commandList = new CommandRepository().GetCommandList(fileName);
        }

        /// <summary>
        /// Destroy
        /// </summary>
        private void OnDestroy()
        {
            _commandList.Clear();
            _commandList = null;
        }
    }
}
