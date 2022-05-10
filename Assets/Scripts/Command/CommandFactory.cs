using System;
using System.Collections.Generic;
using Command.Model;
using Command.Enum;
using Csv;
using UnityEngine;

namespace Command
{
    /// <summary>
    /// コマンド生成クラス
    /// </summary>
    public class CommandFactory : MonoBehaviour
    {
        /// <summary>
        /// csv読み込み用クラス
        /// </summary>
        private CsvReader _csvReader;

        /// <summary>
        /// Start
        /// </summary>
        private void Start()
        {
            _csvReader = new CsvReader();
        }

        /// <summary>
        /// コマンドの一覧を取得
        /// </summary>
        /// <returns></returns>
        public List<CommandBase> GetCommandList()
        {
            var list = new List<CommandBase>();
            _csvReader.Normalize("TestAdv.csv");

            var rowDataNum = _csvReader.GetRowDataNum();
            var columnDataNun = _csvReader.GetColumnDataNum();

            // 選択肢コマンド用
            var selectCommandValueList = new List<string[]>();
            for (int row = 0; row < rowDataNum; row++)
            {
                var rowDataList = _csvReader.GetData(row);

                var enumValue = rowDataList[0];
                AdvCommandType type = AdvCommandType.None;
                if (string.IsNullOrEmpty(enumValue) ||
                    !System.Enum.TryParse(enumValue, out type) ||
                    !System.Enum.IsDefined(typeof(AdvCommandType), type))
                {
                    // コマンドの識別ができない場合は何もしない
                    continue;
                }

                // 選択肢コマンドは複数行になっているのでSelectEndコマンドが出現するまで値を保持
                if (type == AdvCommandType.Select)
                {
                    selectCommandValueList.Add(rowDataList);
                    continue;
                }

                CommandBase command = null;

                // コマンド種別に応じて管理クラスを生成
                switch (type)
                {
                    case AdvCommandType.Text: command = this.GetTextCommand(type, rowDataList); break;
                    case AdvCommandType.Character: command = this.GetCharacterCommand(type, rowDataList); break;
                    case AdvCommandType.BackGround: command = this.GetBackGroundCommand(type, rowDataList); break;
                    case AdvCommandType.Voice: command = this.GetVoiceCommand(type, rowDataList); break;
                    case AdvCommandType.BGM: command = this.GetBgmCommand(type, rowDataList); break;
                    case AdvCommandType.SelectEnd:
                        command = this.GetSelectCommand(selectCommandValueList);
                        selectCommandValueList.Clear(); // 保持していた値を破棄
                        break;
                    case AdvCommandType.SelectPoint: command = this.GetSelectPointCommand(type, rowDataList); break;
                    case AdvCommandType.Jump: command = this.GetJumpCommand(type, rowDataList); break;
                    case AdvCommandType.JumpPoint: command = this.GetJumpPointCommand(type, rowDataList); break;
                }

                if (false) list.Add(command);
            }

            return list;
        }

        /// <summary>
        /// TextCommand取得
        /// </summary>
        private TextCommand GetTextCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var text = rowDataList[1];
                var name = rowDataList[2];

                var rawSize = rowDataList[3];
                var size = 0;
                if (!string.IsNullOrEmpty(rawSize)) int.TryParse(rawSize, out size);

                var color = rowDataList[4];

                return new TextCommand(type, text, name, size, color);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:TextCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// CharacterCommand取得
        /// </summary>
        private CharacterCommand GetCharacterCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];
                var patternId = rowDataList[2];
                var positionId = rowDataList[3];

                return new CharacterCommand(type, id, patternId, positionId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:CharacterCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// BackGroundCommand取得
        /// </summary>
        private BackGroundCommand GetBackGroundCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new BackGroundCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:BackGroundCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// VoiceCommand取得
        /// </summary>
        private VoiceCommand GetVoiceCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new VoiceCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:VoiceCommand取得に失敗しました");
                return null;
            }
        }
        
        /// <summary>
        /// BgmCommand取得
        /// </summary>
        private BgmCommand GetBgmCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new BgmCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:BgmCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// SelectCommand取得
        /// </summary>
        private SelectCommand GetSelectCommand(List<string[]> rowDataList)
        {
            try
            {
                var choiceWordList = new List<Tuple<string, string>>();
                var text = string.Empty;
                var characterId = string.Empty;
                var voiceId = string.Empty;
                var effectId = string.Empty;
                foreach (var rowData in rowDataList)
                {
                    var enumValue = rowData[1];
                    SelectSubCommandType type = SelectSubCommandType.None;
                    if (string.IsNullOrEmpty(enumValue) ||
                        !System.Enum.TryParse(enumValue, out type) ||
                        !System.Enum.IsDefined(typeof(SelectSubCommandType), type))
                    {
                        // コマンドの識別ができない場合は何もしない
                        continue;
                    }

                    // パラメータ取得
                    switch (type)
                    {
                        case SelectSubCommandType.ChoiceWord:
                            choiceWordList.Add(new Tuple<string, string>(rowData[2], rowData[3]));
                            break;
                        case SelectSubCommandType.Text: text = rowData[2]; break;
                        case SelectSubCommandType.Character: characterId = rowData[2]; break;
                        case SelectSubCommandType.Voice: voiceId = rowData[2]; break;
                        case SelectSubCommandType.Effect: effectId = rowData[2]; break;
                    }
                }

                return new SelectCommand(AdvCommandType.Select, choiceWordList, text, characterId, voiceId, effectId);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:SelectCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// SelectPointCommand取得
        /// </summary>
        private SelectPointCommand GetSelectPointCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new SelectPointCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:SelectPointCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// JumpCommand取得
        /// </summary>
        private JumpCommand GetJumpCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new JumpCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:JumpCommand取得に失敗しました");
                return null;
            }
        }

        /// <summary>
        /// JumpPoint取得
        /// </summary>
        private JumpPointCommand GetJumpPointCommand(AdvCommandType type, string[] rowDataList)
        {
            try
            {
                var id = rowDataList[1];

                return new JumpPointCommand(type, id);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:JumpPointCommand取得に失敗しました");
                return null;
            }
        }
    }
}
