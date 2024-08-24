using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;


public class DeepLTranslator
{
    private readonly string _authKey;

    public DeepLTranslator(string authKey)
    {
        _authKey = authKey;
    }

    public async Task<string> TranslateTextAsync(string text, string sourceLanguage, string targetLanguage)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://api-free.deepl.com/v2/translate");

                var parameters = new[]
                {
                    new KeyValuePair<string, string>("auth_key", _authKey),
                    new KeyValuePair<string, string>("text", text),
                    new KeyValuePair<string, string>("source_lang", sourceLanguage),
                    new KeyValuePair<string, string>("target_lang", targetLanguage)
                };

                request.Content = new FormUrlEncodedContent(parameters);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();

                // Deserialize the JSON response
                var responseObject = JsonConvert.DeserializeObject<DeepLResponse>(responseString);

                if (responseObject?.Translations != null && responseObject.Translations.Length > 0)
                {
                    return responseObject?.Translations?[0]?.Text;
                }
                else
                {
                    return "Error translator";
                }


            }
        }

        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}

public class DeepLResponse
{
    [JsonProperty("translations")]
    public Translation[] Translations
    {
        get; set;
    }
}

public class Translation
{
    [JsonProperty("detected_source_language")]
    public string DetectedSourceLanguage
    {
        get; set;
    }

    [JsonProperty("text")]
    public string Text
    {
        get; set;
    }
}
