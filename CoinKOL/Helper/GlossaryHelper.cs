using System;
using Google.Api.Gax;
using Google.Cloud.Translate.V3;

namespace CoinKOL.Helper
{
    public static class GlossaryHelper
    {

        static GlossaryHelper()
        {
        }

        /// <summary>
        /// Create Glossary.
        /// </summary>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        /// <param name="glossaryId">Glossary ID.</param>
        /// <param name="inputUri">Google Cloud Storage URI where glossary is stored in csv format.</param>
        public static void CreateGlossarySample(string projectId = "[Google Cloud Project ID]",
            string glossaryId = "[YOUR_GLOSSARY_ID]",
            string inputUri = "gs://cloud-samples-data/translation/glossary_ja.csv")
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
            CreateGlossaryRequest request = new CreateGlossaryRequest
            {
                ParentAsLocationName = new LocationName(projectId, "us-central1"),
                Parent = new LocationName(projectId, "us-central1").ToString(),
                Glossary = new Glossary
                {
                    Name = new GlossaryName(projectId, "us-central1", glossaryId).ToString(),
                    LanguageCodesSet = new Glossary.Types.LanguageCodesSet
                    {
                        LanguageCodes =
                        {
                            "en", // source lang
                            "ja", // target lang
                        },
                    },
                    InputConfig = new GlossaryInputConfig
                    {
                        GcsSource = new GcsSource
                        {
                            InputUri = inputUri,
                        },
                    },
                },
            };
            // Poll until the returned long-running operation is complete
            Glossary response = translationServiceClient.CreateGlossary(request).PollUntilCompleted().Result;
            Console.WriteLine("Created Glossary.");
            Console.WriteLine($"Glossary name: {response.Name}");
            Console.WriteLine($"Entry count: {response.EntryCount}");
            Console.WriteLine($"Input URI: {response.InputConfig.GcsSource.InputUri}");
        }

        /// <summary>
        /// Deletes a Glossary.
        /// </summary>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        /// <param name="glossaryId">Glossary ID.</param>
        public static void DeleteGlossarySample(string projectId = "[Google Cloud Project ID]",
            string glossaryId = "[YOUR_GLOSSARY_ID]")
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
            DeleteGlossaryRequest request = new DeleteGlossaryRequest
            {
                GlossaryName = new GlossaryName(projectId, "us-central1", glossaryId),
            };
            // Poll until the returned long-running operation is complete
            DeleteGlossaryResponse response = translationServiceClient.DeleteGlossary(request).PollUntilCompleted().Result;
            Console.WriteLine("Deleted Glossary.");
        }

        /// <summary>
        /// Retrieves a glossary.
        /// </summary>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        /// <param name="glossaryId">Glossary ID.</param>
        public static void GetGlossarySample(string projectId = "[Google Cloud Project ID]",
            string glossaryId = "[YOUR_GLOSSARY_ID]")
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
            GetGlossaryRequest request = new GetGlossaryRequest
            {
                GlossaryName = new GlossaryName(projectId, "us-central1", glossaryId),
            };
            Glossary response = translationServiceClient.GetGlossary(request);
            Console.WriteLine($"Glossary name: {response.Name}");
            Console.WriteLine($"Entry count: {response.EntryCount}");
            Console.WriteLine($"Input URI: {response.InputConfig.GcsSource.InputUri}");
        }

        /// <summary>
        /// Lists all the glossaries for a given project.
        /// </summary>
        /// <param name="projectId">Your Google Cloud Project ID.</param>
        public static void ListGlossariesSample(string projectId = "[Google Cloud Project ID]")
        {
            TranslationServiceClient translationServiceClient = TranslationServiceClient.Create();
            ListGlossariesRequest request = new ListGlossariesRequest
            {
                ParentAsLocationName = new LocationName(projectId, "us-central1"),
            };
            PagedEnumerable<ListGlossariesResponse, Glossary> response = translationServiceClient.ListGlossaries(request);
            // Iterate over glossaries and display each glossary's details.
            foreach (Glossary item in response)
            {
                Console.WriteLine($"Glossary name: {item.Name}");
                Console.WriteLine($"Entry count: {item.EntryCount}");
                Console.WriteLine($"Input URI: {item.InputConfig.GcsSource.InputUri}");
            }
        }
    }
}

