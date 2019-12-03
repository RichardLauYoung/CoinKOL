using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Google.Cloud.Translate.V3;

namespace CoinKOL.Services
{
    /// <summary>
    /// 翻译
    /// </summary>
    public static class TranslationService
    {
        static TranslationService()
        {

        }

        /// <summary>
        /// 翻译任务
        /// </summary>
        /// <returns></returns>
        static bool Translation(string text)
        {
            //获取数据

            //翻译
            TranslateText(text);

            //保存数据
            return true;
        }


        /// <summary>
        /// 文本翻译（默认英文转中文）
        /// </summary>
        /// <param name="text">需要翻译的文本</param>
        /// <returns></returns>
        private static string TranslateText(string text, string fl = "zh", string tl = "en_US")
        {
            var url = String.Format
            ("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}",
             fl, tl, Uri.EscapeUriString(text));

            var httpClient = new HttpClient();
            var result = httpClient.GetStringAsync(url).Result;
            var jsonData = JsonSerializer.Deserialize<List<dynamic>>(result);
            var translationItems = jsonData[0];
            string translation = "";
            foreach (object item in translationItems)
            {
                IEnumerable translationLineObject = item as IEnumerable;

                IEnumerator translationLineString = translationLineObject.GetEnumerator();

                translationLineString.MoveNext();
                translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            }

            if (translation.Length > 1) { translation = translation.Substring(1); }

            return translation;
        }
    }
}

