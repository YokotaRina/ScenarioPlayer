using System;
using System.Collections.Generic;
using Model.Sound;
using Model.Texture;
using UnityEngine;

namespace Model.Factory
{
    /// <summary>
    /// リソース関連の情報生成クラス
    /// </summary>
    public class ResourceFactory
    {
        /// <summary>
        /// CSVデータから背景情報リストを生成する
        /// </summary>
        public List<BackGroundBase> CreateBackGroundList(List<string[]> rawDataList)
        {
            var list = new List<BackGroundBase>();
            if (rawDataList == null || rawDataList.Count == 0) return list;

            // csvデータからコマンドリストを生成する
            foreach (var rawDara in rawDataList)
            {
                try
                {
                    var id = rawDara[0];
                    var resource = rawDara[1];

                    if (!string.IsNullOrEmpty(id))
                    {
                        list.Add(new BackGroundBase(id, resource));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[{ex}]:BackGroundBase取得に失敗しました");
                }
            }

            return list;
        }

        /// <summary>
        /// CSVデータからボイス情報リストを生成する
        /// </summary>
        public List<VoiceBase> CreateVoiceList(List<string[]> rawDataList)
        {
            var list = new List<VoiceBase>();
            if (rawDataList == null || rawDataList.Count == 0) return list;

            // csvデータからコマンドリストを生成する
            foreach (var rawDara in rawDataList)
            {
                try
                {
                    var id = rawDara[0];
                    var resource = rawDara[1];

                    if (!string.IsNullOrEmpty(id))
                    {
                        list.Add(new VoiceBase(id, resource));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[{ex}]:VoiceBase取得に失敗しました");
                }
            }

            return list;
        }

        /// <summary>
        /// CSVデータからBgm情報リストを生成する
        /// </summary>
        public List<BgmBase> CreateBgmList(List<string[]> rawDataList)
        {
            var list = new List<BgmBase>();
            if (rawDataList == null || rawDataList.Count == 0) return list;

            // csvデータからコマンドリストを生成する
            foreach (var rawDara in rawDataList)
            {
                try
                {
                    var id = rawDara[0];
                    var resource = rawDara[1];

                    var rawIsLoop = rawDara[2];
                    var isLoop = false;
                    if (!string.IsNullOrEmpty(rawIsLoop)) bool.TryParse(rawIsLoop, out isLoop);

                    if (!string.IsNullOrEmpty(id))
                    {
                        list.Add(new BgmBase(id, resource, isLoop));
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[{ex}]:BgmBase取得に失敗しました");
                }
            }

            return list;
        }
    }
}
