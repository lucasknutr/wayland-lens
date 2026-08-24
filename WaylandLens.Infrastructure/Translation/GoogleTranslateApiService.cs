using System.Text.Json;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Translation;

public class GoogleTranslateApiService(HttpClient httpClient) : ITranslationService
{
    public async Task<string> TranslateAsync(string text, string targetLanguage)
    { 
        // Uri.EspapeDataString() method will "URL encode" our text
        string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";

        var responseString = await httpClient.GetStringAsync(url);

        using var document = JsonDocument.Parse(responseString);

        var translatedText = document.RootElement[0][0][0].GetString();
        
        return translatedText;
    }
}