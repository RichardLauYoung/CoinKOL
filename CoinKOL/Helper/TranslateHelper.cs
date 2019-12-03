using System;
using Google.Cloud.Translate.V3;

namespace CoinKOL.Helper
{
    public static class TranslateHelper
    {
        static TranslateHelper()
        {
        }

        /// <summary>
        /// Translates a given text to a target language.
        /// </summary>
        /// <param name="text">The content to translate.</param>
        /// <param name="sourceLanguage">Required. Source language code.</param>
        /// <param name="targetLanguage">Required. Target language code.</param>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        public static void TranslateTextSample(string text,string sourceLanguage,
            string targetLanguage,
            string projectId)
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents =
                {
                    // The content to translate.
                    text,
                },
                SourceLanguageCode = sourceLanguage,
                TargetLanguageCode = targetLanguage,
                ParentAsLocationName = new LocationName(projectId, "global"),
            };
            TranslateTextResponse response = translationServiceClient.TranslateText(request);
            // Display the translation for each input text provided
            foreach (Translation translation in response.Translations)
            {
                Console.WriteLine($"Translated text: {translation.TranslatedText}");
            }
        }

        /// <summary>
        /// Translates a given text to a target language using glossary.
        /// </summary>
        /// <param name="text">The content to translate.</param>
        /// <param name="sourceLanguage">Optional. Source language code.</param>
        /// <param name="targetLanguage">Required. Target language code.</param>
        /// <param name="glossaryId">Translation Glossary ID.</param>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        public static void TranslateTextWithGlossarySample(
            string text = "[TEXT_TO_TRANSLATE]",
            string sourceLanguage = "en",
            string targetLanguage = "ja",
            string projectId = "[Google Cloud Project ID]",
            string glossaryId = "[YOUR_GLOSSARY_ID]")
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();

            string glossaryPath = $"projects/{projectId}/locations/{"us-central1"}/glossaries/{glossaryId}";
            TranslateTextRequest request = new TranslateTextRequest
            {
                Contents =
                {
                    // The content to translate.
                    text,
                },
                TargetLanguageCode = targetLanguage,
                ParentAsLocationName = new LocationName(projectId, "us-central1"),
                SourceLanguageCode = sourceLanguage,
                GlossaryConfig = new TranslateTextGlossaryConfig
                {
                    // Translation Glossary Path.
                    Glossary = glossaryPath,
                },
                MimeType = "text/plain",
            };
            TranslateTextResponse response = translationServiceClient.TranslateText(request);
            // Display the translation for given content.
            foreach (Translation translation in response.GlossaryTranslations)
            {
                Console.WriteLine($"Translated text: {translation.TranslatedText}");
            }
        }
    }
}
