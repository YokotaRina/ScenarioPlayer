using System.Collections.Generic;
using Csv;
using Model.Factory;
using Model.Sound;
using Model.Texture;

namespace Repository
{
    /// <summary>
    /// リソース関連の情報のリポジトリ
    /// </summary>
    public class ResourceRepository
    {
        /// <summary>
        /// 背景情報リストを取得
        /// </summary>
        public List<BackGroundBase> GetBackGroundList()
        {
            // csv読み込み
            var csvReader = new CsvReader();
            csvReader.Normalize(CsvReader.CsvType.Resource, "BackGround.csv");

            var list = csvReader.GetData();
            list.RemoveAt(0); // 1行目はヘッダーのため削除
            return new ResourceFactory().CreateBackGroundList(list);
        }

        /// <summary>
        /// ボイス情報リストを取得
        /// </summary>
        public List<VoiceBase> GetVoiceList()
        {
            // csv読み込み
            var csvReader = new CsvReader();
            csvReader.Normalize(CsvReader.CsvType.Resource, "Voice.csv");

            var list = csvReader.GetData();
            list.RemoveAt(0); // 1行目はヘッダーのため削除
            return new ResourceFactory().CreateVoiceList(list);
        }
    }
}
