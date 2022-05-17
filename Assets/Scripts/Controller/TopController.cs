using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    /// <summary>
    /// Topのコントローラー
    /// </summary>
    public class TopController : MonoBehaviour
    {
        [SerializeField, Tooltip("スタートボタン")] private Button startButton = default;
        [SerializeField, Tooltip("再生ファイル一覧")] private Dropdown fileList = default;
        [SerializeField, Tooltip("TOPオブジェクト")] private GameObject topObject = default;
        [SerializeField, Tooltip("ADVオブジェクト")] private AdvController advController = default;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            // DropDownの設定
            {
                var list = new List<string>();
                fileList.options.Clear();

                // シナリオファイル一覧を取得
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "Assets/MasterData/Scenario");
                string[] fullPathList = Directory.GetFiles(directory, "*.csv");
                foreach (string fullPath in fullPathList)
                {
                    list.Add(Path.GetFileName(fullPath));
                }

                fileList.AddOptions(list);
            }

            // ボタンの設定
            startButton.onClick.AddListener(OnStartButton);
        }

        /// <summary>
        /// Startボタン押下時
        /// </summary>
        private void OnStartButton()
        {
            // 画面切り替え
            this.gameObject.SetActive(false);
            advController.gameObject.SetActive(true);

            // ファイル名設定
            var fileName = fileList.options[fileList.value].text;
            advController.SetUp(fileName);
        }
    }
}
