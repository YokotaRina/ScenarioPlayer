using System.Collections.Generic;
using Command;
using Command.Model;
using Csv;
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
        private void Initialize(string fileName)
        {
            var csvReader = new CsvReader();
            var factory = new CommandFactory();

            // コマンドリストの取得
            csvReader.Normalize(fileName);
            _commandList = factory.GetCommandList(csvReader);
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
