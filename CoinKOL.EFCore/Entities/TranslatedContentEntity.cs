using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinKOL.EFCore.Entities
{
    [Table("kol_translated_content")]
    public class TranslatedContentEntity
    {
        public TranslatedContentEntity()
        {
            
        }
        //Id INT PRIMARY KEY IDENTITY(1,1),
        public int ID { get; set; }

        //SourceText VARCHAR(MAX),--内容英文原文
        public string SourceText { get; set; }

        //TranslatedText VARCHAR(MAX),--翻译后的文本
        public string TranslatedText { get; set; }

        //Author VARCHAR(32),--发布内容的人
        public string Author { get; set; }

        //SourceURL VARCHAR(256),--源文链接
        public string SourceURL { get; set; }

        //SourceLanguage VARCHAR(10),--原文语言
        public string SourceLanguage { get; set; }

        //TargetLanguage VARCHAR(10),--目标翻译语言
        public string TargetLanguage { get; set; }

        //GlossaryId VARCHAR(128),--使用的术语库的编号
        public string GlossaryId { get; set; }

        //CreteTime DATETIME,--内容创建时间
        public DateTime CreteTime { get; set; }

        //Level INT --内容重要等级 1：极其重要 2：重要 3：一般
        public int Level { get; set; }
    }
}
