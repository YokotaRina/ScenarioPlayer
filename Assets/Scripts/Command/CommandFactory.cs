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
    public class CommandFactory : IDisposable
    {
        /// <summary>
        /// コマンド管理クラス生成前の中間データ
        /// </summary>
        private class CommandIntermediateData
        {
            /// <summary>
            /// コマンド種別
            /// </summary>
            public AdvCommandType Type { get; private set; }

            /// <summary>
            /// 一コマンド生成に必要なデータリスト
            /// </summary>
            public List<string[]> DataList { get; private set; }

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CommandIntermediateData(AdvCommandType type, List<string[]> dataList)
            {
                Type = type;
                DataList = dataList;
            }
        }

        /// <summary>
        /// 中間データのリスト
        /// </summary>
        private List<CommandIntermediateData> _intermediateDataList;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public CommandFactory()
        {
            _intermediateDataList = new List<CommandIntermediateData>();
        }

        /// <summary>
        /// コマンドの一覧を取得
        /// </summary>
        /// <returns></returns>
        public List<CommandBase> GetCommandList(CsvReader csvReader)
        {
            // 中間データの生成
            this.CreateIntermediateData(csvReader);

            // コマンドリストの生成
            return this.CreateCommandList();
        }

        /// <summary>
        /// 中間データ生成
        /// </summary>
        private void CreateIntermediateData(CsvReader csvReader)
        {
            var rowDataNum = csvReader.GetRowDataNum();
            var dataList = new List<string[]>();

            // csvから中間データを生成する
            for (int row = 0; row < rowDataNum; row++)
            {
                var rowDataList = csvReader.GetData(row);

                var enumValue = rowDataList[0];
                if (string.IsNullOrEmpty(enumValue)) continue; // 空文字の場合は何もしない
                if (!System.Enum.TryParse(enumValue, out AdvCommandType type) || // AdvCommandTypeに変換を試みる
                    !System.Enum.IsDefined(typeof(AdvCommandType), type)) // 変換できた場合、定義されているか確認
                {
                    // コマンドの識別ができない場合は何もしない
                    continue;
                }

                // データ注入
                dataList.Add(rowDataList);

                // 選択肢コマンドは複数行になっているのでSelectEndコマンドが出現するまで値を保持
                if (type == AdvCommandType.Select) continue;

                // 中間データ生成
                _intermediateDataList.Add(new CommandIntermediateData(type, new List<string[]>(dataList)));
                dataList.Clear();
            }
        }

        /// <summary>
        /// コマンドリストの生成
        /// </summary>
        private List<CommandBase> CreateCommandList()
        {
            var list = new List<CommandBase>();
            if (_intermediateDataList == null || _intermediateDataList.Count == 0) return list;

            // 中間データからコマンドリストを生成する
            foreach (var intermediateData in _intermediateDataList)
            {
                CommandBase command = null;
                var type = intermediateData.Type;
                var dataList = intermediateData.DataList;

                if (dataList == null || dataList.Count == 0) continue;

                // コマンド種別に応じて管理クラスを生成
                switch (type)
                {
                    case AdvCommandType.Text: command = this.GetTextCommand(type, dataList[0]); break;
                    case AdvCommandType.Character: command = this.GetCharacterCommand(type, dataList[0]); break;
                    case AdvCommandType.BackGround: command = this.GetBackGroundCommand(type, dataList[0]); break;
                    case AdvCommandType.Voice: command = this.GetVoiceCommand(type, dataList[0]); break;
                    case AdvCommandType.BGM: command = this.GetBgmCommand(type, dataList[0]); break;
                    case AdvCommandType.SelectEnd: command = this.GetSelectCommand(dataList); break;
                    case AdvCommandType.SelectPoint: command = this.GetSelectPointCommand(type, dataList[0]); break;
                    case AdvCommandType.Jump: command = this.GetJumpCommand(type, dataList[0]); break;
                    case AdvCommandType.JumpPoint: command = this.GetJumpPointCommand(type, dataList[0]); break;
                }

                if (command != null) list.Add(command);    
            }

            return list;
        }

# region Command生成
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
                    if (string.IsNullOrEmpty(enumValue)) continue; // 空文字の場合は何もしない
                    if (!System.Enum.TryParse(enumValue, out type) || // SelectSubCommandTypeに変換を試みる
                        !System.Enum.IsDefined(typeof(SelectSubCommandType), type)) // 変換できた場合、定義されているか確認
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
# endregion

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _intermediateDataList.Clear();
        }
    }
}
