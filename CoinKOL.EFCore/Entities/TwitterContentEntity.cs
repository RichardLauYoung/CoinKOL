using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoinKOL.EFCore.Entities
{
    [Table("kol_twitter_content")]
    public class TwitterContentEntity
    {
        public TwitterContentEntity()
        {
        }
        //Id INT PRIMARY KEY IDENTITY(1,1),
        public int Id { get; set; }

        //Text VARCHAR(MAX),--Twitter 内容
        public string Text { get; set; }

        //Author VARCHAR(32),--发布内容的人
        public string Author { get; set; }

        //SourceURL VARCHAR(256),--源文链接
        public string SourceURL { get; set; }

        //CreteTime DATETIME,--内容创建的时间
        public DateTime CreteTime { get; set; }

        //CaptureTime DATETIME,--数据捕获的时间
        public DateTime CaptureTime { get; set; }

        //IsTranslated INT,--是否被翻译过 1：翻译过的 0：没翻译的
        public int IsTranslated { get; set; }

        //TranslateTime DATETIME,--内容翻译时间
        public DateTime TranslateTime { get; set; }

        //Level INT,--内容重要等级
        public int Level { get; set; }
    }
}
