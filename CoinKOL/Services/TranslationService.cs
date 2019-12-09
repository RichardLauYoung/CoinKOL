using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using CoinKOL.EFCore.Context;
using CoinKOL.Helper;
using Google.Cloud.Translate.V3;
using Microsoft.Extensions.Configuration;

namespace CoinKOL.Services
{
    /// <summary>
    /// 翻译
    /// </summary>
    public static class TranslationService
    {
        private static string ProjectId { get; set; }

        static TranslationService()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();
            ProjectId = configuration.GetSection("Google_Project_Config:ProjectId").Value;
        }

        /// <summary>
        /// 翻译任务
        /// </summary>
        /// <returns></returns>
        public static bool Translation(string text)
        {
            //获取数据
            
            
            //翻译
            //TranslateText(text);
            TranslateHelper.TranslateTextSample(text, ProjectId);

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

