using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace JMTOYStranslator.Functions
{
    public class LanguageTools
    {
        public static List<LanguageData> LoadLanguages( )
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream($"JMTOYStranslator.{"LanguagesSupp.json"}"))
            {
                if (stream == null)
                    throw new FileNotFoundException("Resource not found.", "LanguagesSupp.json");

                using (var reader = new StreamReader(stream))
                {
                    var jsonString = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<LanguageDep>(jsonString).Translations.ToList();
                }
            }
        }
    }

    public class LanguageData
    {
        [JsonProperty("language")]
        public string LanguageCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("supports_formality")]
        public bool SupportsFormality { get; set; }
    }

    public class LanguageDep
    {
        [JsonProperty("idiomas")]
        public LanguageData[] Translations
        {
            get; set;
        }
    }
}
