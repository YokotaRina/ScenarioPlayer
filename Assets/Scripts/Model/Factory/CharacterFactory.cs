using System;
using System.Collections.Generic;
using Enums;
using Model.Character;
using Repository;
using UnityEngine;

namespace Model.Factory
{
    /// <summary>
    /// キャラクター生成クラス
    /// </summary>
    public class CharacterFactory
    {
        /// <summary>
        /// 中間データからキャラクターリストを生成する
        /// </summary>
        public List<CharacterBase> CreateCharacterList(List<CharacterRepository.CharacterIntermediateData> intermediateDataList)
        {
            var list = new List<CharacterBase>();
            if (intermediateDataList == null || intermediateDataList.Count == 0) return list;

            // 中間データからコマンドリストを生成する
            foreach (var intermediateData in intermediateDataList)
            {
                var id = intermediateData.Id;
                var name = intermediateData.Name;
                var dataList = intermediateData.DataList;

                if (dataList == null || dataList.Count == 0) continue;

                // 表情パターンとリソースの紐付けディクショナリを生成
                var resourceDictionary = new Dictionary<FacePattern, string>();
                foreach (var data in dataList)
                {
                    try
                    {
                        var enumValue = data[2];
                        if (string.IsNullOrEmpty(enumValue)) continue; // 空文字の場合は何もしない
                        if (!Enum.TryParse(enumValue, out FacePattern type) || // FacePatternに変換を試みる
                            !Enum.IsDefined(typeof(FacePattern), type)) // 変換できた場合、定義されているか確認
                        {
                            // FacePatternの識別ができない場合は何もしない
                            continue;
                        }

                        var resource = data[3];
                        resourceDictionary[type] = resource;
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"[{ex}]:CharacterBase取得に失敗しました");
                    }
                }

                if (!string.IsNullOrEmpty(name) && resourceDictionary.Count != 0)
                {
                    list.Add(new CharacterBase(id, name, resourceDictionary));
                }    
            }

            return list;
        }

        /// <summary>
        /// CSVデータからキャラクター表示位置リストを生成する
        /// </summary>
        public List<PositionBase> CreatePositionList(List<string[]> rawDataList)
        {
            var list = new List<PositionBase>();
            if (rawDataList == null || rawDataList.Count == 0) return list;

            // csvデータからコマンドリストを生成する
            foreach (var rawDara in rawDataList)
            {
                try
                {
                    var id = rawDara[0];

                    var rawX = rawDara[1];
                    var x = 0;
                    if (!string.IsNullOrEmpty(rawX)) int.TryParse(rawX, out x);

                    var rawY = rawDara[2];
                    var y = 0;
                    if (!string.IsNullOrEmpty(rawY)) int.TryParse(rawY, out y);

                    var rawOrder = rawDara[3];
                    var order = 0;
                    if (!string.IsNullOrEmpty(rawOrder)) int.TryParse(rawOrder, out order);

                    if (!string.IsNullOrEmpty(id))
                    {
                        list.Add(new PositionBase(id, x, y, order));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[{ex}]:PositionBase取得に失敗しました");
                }
            }

            return list;
        }
    }
}
