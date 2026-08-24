using System.Text.Json;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Translation;

public class GoogleTranslateApiService(HttpClient httpClient) : ITranslationService
{
    public async Task<string> TranslateAsync(string text, string targetLanguage)
    { 
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        
        // Uri.EspapeDataString() method will "URL encode" our text
        string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLanguage}&dt=t&q={Uri.EscapeDataString(text)}";
        var responseString = await httpClient.GetStringAsync(url);
        using var document = JsonDocument.Parse(responseString);

        var sb = new System.Text.StringBuilder();
        
        foreach (var element in document.RootElement[0].EnumerateArray())
        {
            sb.Append(element[0].GetString());
        }

        return sb.ToString() ?? string.Empty;
    }
}