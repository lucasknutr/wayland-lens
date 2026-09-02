using System.Text.Json;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Translation;

public class DeepLTranslationService(HttpClient httpClient) : ITranslationService
{
    public async Task<string> TranslateAsync(string text, string targetLanguage)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var requestBody = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["text"] = text,
            ["target_lang"] = targetLanguage.ToUpperInvariant()
        });

        var response = await httpClient.PostAsync("https://api-free.deepl.com/v2/translate", requestBody);
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(responseString);

        return document.RootElement.GetProperty("translations")[0].GetProperty("text").GetString() ?? string.Empty;
    }
}
