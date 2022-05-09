using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CsvReader
{
    /// <summary>
    /// CSVの読み込み
    /// </summary>
    public class CsvReader : MonoBehaviour
    {
        /// <summary>
        /// コメントコマンド判定用
        /// </summary>
        private static string COMMENT_COMMAND = "Comment";

        /// <summary>
        /// ファイル名
        /// </summary>
        private string _fileName;

        /// <summary>
        /// CSVの中身の文字列データリスト
        /// </summary>
        private readonly List<string[]> _dataList = new List<string[]>();

        private void Start()
        {
            Load("TestAdv.csv");
        }

        /// <summary>
        /// 読み込み
        /// </summary>
        private void Load(string fileName)
        {
            _fileName = fileName;

            // 読み込み
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Scenario");
            var streamReader = new StreamReader($"{path}/{_fileName}");

            // 末尾まで繰り返す
            string line = "";
            while (!streamReader.EndOfStream)
            {
                // CSVファイルの一行を読み込む
                line += streamReader.ReadLine();
 
                // フィールドを示す為のダブルクォーテーションを数える
                var doubleQuotationCount = Regex.Split(line, "\"").Length - 1;
                // 奇数なら次の行も同じフィールドとみなす
                if (doubleQuotationCount % 2 == 1)
                {
                    // 文末の改行コードは行を読み込む際に消されているので補完
                    line += "\n";
                    continue;
                }

                // ダブルクォーテションを排除し、フィールドを区切るカンマ毎に分ける
                line = line.Replace("\"", "");
                var valueList = line.Split(',');
 
                // Comment行の場合は何もしない
                if (valueList[0] == COMMENT_COMMAND)
                {
                    line = "";
                    continue;
                }

                // コンソールに出力する
                // foreach (string value in valueList)
                // {
                //     UnityEngine.Debug.LogError(value);
                // }
                _dataList.Add(valueList);
                line = "";
            }
        }

        /// <summary>
        /// 行数を取得
        /// </summary>
        /// <returns></returns>
        public int GetRowDataNum()
        {
            return _dataList.Count;
        }

        /// <summary>
        /// 列数を取得
        /// </summary>
        /// <returns></returns>
        public int GetColumnDataNum()
        {
            return _dataList == null || _dataList.Count == 0 ? 0 : _dataList[0].Length;
        }

        /// <summary>
        /// 全てのデータを取得
        /// </summary>
        /// <returns></returns>
        public List<string[]> GetData()
        {
            return _dataList;
        }

        /// <summary>
        /// 指定行のデータを取得
        /// </summary>
        public string[] GetData(int row)
        {
            var valueArray = new string[]{};
            try
            {
                valueArray = _dataList[row];
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:{_fileName}_{row+1}行目のCSVのデータ取得に失敗しました");
            }
            return valueArray;
        }

        /// <summary>
        /// 指定行の指定列のデータを取得
        /// </summary>
        public string GetData(int row, int column)
        {
            var value = String.Empty;
            try
            {
                value = _dataList[row][column];
            }
            catch (Exception ex)
            {
                Debug.LogError($"[{ex}]:{_fileName}_{row+1}行目_{column+1}列目のCSVのデータ取得に失敗しました");
            }
            return value;
        }
    }
}
