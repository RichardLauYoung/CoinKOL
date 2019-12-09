using System;
using CoinKOL.EFCore.Context;
using CoinKOL.EFCore.Entities;
using CoinKOL.EFCore.Interfacee;

namespace CoinKOL.EFCore
{
    public class KolDbContext : IKolDbContext
    {
        public bool SavaTranslateText(TranslatedContentEntity translatedContent)
        {
            using (var db = new Kol_DbContext())
            {
                db.TranslatedContent.Add(translatedContent);
                var count = db.SaveChanges();
                return count > 0;
            }
        }
    }
}
