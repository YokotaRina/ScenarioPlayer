using System.Collections.Generic;
using Command.Enum;
using Command.Model;
using Csv;

namespace Command
{
    /// <summary>
    /// コマンドのリポジトリ
    /// </summary>
    public class CommandRepository
    {
        /// <summary>
        /// コマンド管理クラス生成前の中間データ
        /// </summary>
        public class CommandIntermediateData
        {
            /// <summary>
            /// コマンド種別
            /// </summary>
            public readonly AdvCommandType Type;

            /// <summary>
            /// 一コマンド生成に必要なデータリスト
            /// </summary>
            public readonly List<string[]> DataList;

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
        /// コマンドリストを取得
        /// </summary>
        public List<CommandBase> GetCommandList(string fileName)
        {
            var intermediateDataList = this.CreateIntermediateData(fileName);
            return new CommandFactory().CreateCommandList(intermediateDataList);
        }

        /// <summary>
        /// CSVから中間データ生成
        /// </summary>
        private List<CommandIntermediateData> CreateIntermediateData(string fileName)
        {
            // csv読み込み
            var csvReader = new CsvReader();
            csvReader.Normalize(fileName);

            var intermediateDataList = new List<CommandIntermediateData>();
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
                intermediateDataList.Add(new CommandIntermediateData(type, new List<string[]>(dataList)));
                dataList.Clear();
            }

            return intermediateDataList;
        }
    }
}
