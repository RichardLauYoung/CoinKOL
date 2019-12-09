using System;
using CoinKOL.EFCore.Entities;

namespace CoinKOL.EFCore.Interfacee
{
    public interface IKolDbContext
    {
        public bool SavaTranslateText(TranslatedContentEntity translatedContent);
    }
}
