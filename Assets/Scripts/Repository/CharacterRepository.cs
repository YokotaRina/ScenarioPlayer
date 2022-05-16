using System.Collections.Generic;
using System.Linq;
using Csv;
using Model.Character;
using Model.Factory;

namespace Repository
{
    /// <summary>
    /// キャラクターのリポジトリ
    /// </summary>
    public class CharacterRepository
    {
        /// <summary>
        /// キャラクター情報保持クラス生成前の中間データ
        /// </summary>
        public class CharacterIntermediateData
        {
            /// <summary>
            /// Id
            /// </summary>
            public readonly string Id;

            /// <summary>
            /// 名前
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// 一キャラクター情報生成に必要なデータリスト
            /// </summary>
            public readonly List<string[]> DataList;

            /// <summary>
            /// スケール
            /// </summary>
            public readonly int Scale;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            public CharacterIntermediateData(string id, string name, List<string[]> dataList, int scale)
            {
                Id = id;
                Name = name;
                DataList = dataList;
                Scale = scale;
            }
        }

        /// <summary>
        /// キャラクターリストを取得
        /// </summary>
        public List<CharacterBase> GetCharacterList()
        {
            var intermediateDataDictionary = this.CreateCharacterIntermediateData();
            return new CharacterFactory().CreateCharacterList(intermediateDataDictionary);
        }

        /// <summary>
        /// CSVから中間データ生成
        /// </summary>
        private List<CharacterIntermediateData> CreateCharacterIntermediateData()
        {
            // csv読み込み
            var csvReader = new CsvReader();
            csvReader.Normalize(CsvReader.CsvType.Character, "Character.csv");

            var intermediateDataList = new List<CharacterIntermediateData>();
            var rowDataNum = csvReader.GetRowDataNum();

            // csvから中間データを生成する
            // ※1行目はヘッダーのため無視
            var beforeId = string.Empty;
            for (int row = 1; row < rowDataNum; row++)
            {
                var rowDataList = csvReader.GetData(row);

                var id = rowDataList[0];
                var name = rowDataList[1];

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name)) continue; // idが空の場合は何もしない

                var rawScale = rowDataList[3];
                var scale = 0;
                if (!string.IsNullOrEmpty(rawScale)) int.TryParse(rawScale, out scale);

                // 一行前とIDが同じ場合
                if (beforeId == id)
                {
                    var data = intermediateDataList.FirstOrDefault(x => x.Id == id);
                    data?.DataList.Add(rowDataList);
                }
                else
                {
                    intermediateDataList.Add(new CharacterIntermediateData(id, name, new List<string[]> {rowDataList}, scale));
                }

                beforeId = id;
            }

            return intermediateDataList;
        }

        /// <summary>
        /// キャラクター表示位置リストを取得
        /// </summary>
        public List<PositionBase> GetPositionList()
        {
            // csv読み込み
            var csvReader = new CsvReader();
            csvReader.Normalize(CsvReader.CsvType.Character, "CharacterPosition.csv");

            var list = csvReader.GetData();
            list.RemoveAt(0); // 1行目はヘッダーのため削除
            return new CharacterFactory().CreatePositionList(list);
        }
    }
}
