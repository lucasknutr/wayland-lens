using System.Diagnostics;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Speech;

public class AzureSpeechService(HttpClient httpClient) : ISpeechService
{
    private static readonly Dictionary<string, (string Locale, string Voice)> VoicesByLanguage = new()
    {
        ["ru"] = ("ru-RU", "ru-RU-DmitryNeural")
    };

    public async Task SpeakAsync(string text, string language)
    {
        if (!VoicesByLanguage.TryGetValue(language, out var voice))
            throw new InvalidOperationException($"No Azure voice configured for language '{language}'.");

        var ssml = $"<speak version='1.0' xml:lang='{voice.Locale}'><voice name='{voice.Voice}'>{SecurityElement.Escape(text)}</voice></speak>";

        using var request = new HttpRequestMessage(HttpMethod.Post, "cognitiveservices/v1")
        {
            Content = new StringContent(ssml, Encoding.UTF8, "application/ssml+xml")
        };
        request.Headers.Add("X-Microsoft-OutputFormat", "riff-24khz-16bit-mono-pcm");
        request.Headers.UserAgent.Add(new ProductInfoHeaderValue("WaylandLens", "1.0"));

        var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var audioBytes = await response.Content.ReadAsByteArrayAsync();
        var wavPath = Path.GetTempFileName();
        await File.WriteAllBytesAsync(wavPath, audioBytes);

        var playInfo = new ProcessStartInfo
        {
            FileName = "pw-play",
            UseShellExecute = false,
            CreateNoWindow = true
        };
        playInfo.ArgumentList.Add(wavPath);

        using var playProcess = Process.Start(playInfo)!;
        await playProcess.WaitForExitAsync();

        File.Delete(wavPath);
    }
}
