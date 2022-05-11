using System;
using System.Collections.Generic;
using Enums;
using Model.Command;
using Repository;
using UnityEngine;

namespace Model.Factory
{
    /// <summary>
    /// コマンド生成クラス
    /// </summary>
    public class CommandFactory
    {
        /// <summary>
        /// 中間データからコマンドリストを生成する
        /// </summary>
        public List<CommandBase> CreateCommandList(List<CommandRepository.CommandIntermediateData> intermediateDataList)
        {
            var list = new List<CommandBase>();
            if (intermediateDataList == null || intermediateDataList.Count == 0) return list;

            // 中間データからコマンドリストを生成する
            foreach (var intermediateData in intermediateDataList)
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

# region 各Commandクラスの生成
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
                    if (string.IsNullOrEmpty(enumValue)) continue; // 空文字の場合は何もしない
                    if (!System.Enum.TryParse(enumValue, out SelectSubCommandType type) || // SelectSubCommandTypeに変換を試みる
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
    }
}
