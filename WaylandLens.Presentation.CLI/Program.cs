using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WaylandLens.Application.UseCases;
using WaylandLens.Domain.Interfaces;
using WaylandLens.Infrastructure.Ocr;
using WaylandLens.Infrastructure.Translation;
using WaylandLens.Infrastructure.Wayland;
using WaylandLens.Infrastructure.Speech;

var silent = args.Contains("--silent");

var services = new ServiceCollection();

services.AddTransient<IScreenCaptureService, GrimSlurpCaptureService>();
services.AddTransient<IOcrService, TesseractCliOcrService>();
services.AddTransient<RunTranslationUseCase>();
services.AddHttpClient<ITranslationService, DeepLTranslationService>(client =>
{
    var apiKey = Environment.GetEnvironmentVariable("DEEPL_API_KEY")
        ?? throw new InvalidOperationException("DEEPL_API_KEY environment variable is not set.");
    client.DefaultRequestHeaders.Add("Authorization", $"DeepL-Auth-Key {apiKey}");
});

// Azure Speech resource region - update this if you ever recreate the resource in a different region
services.AddHttpClient<ISpeechService, AzureSpeechService>(client =>
{
    var apiKey = Environment.GetEnvironmentVariable("AZURE_SPEECH_KEY")
        ?? throw new InvalidOperationException("AZURE_SPEECH_KEY environment variable is not set.");
    client.BaseAddress = new Uri("https://brazilsouth.tts.speech.microsoft.com/");
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
});

var serviceProvider = services.BuildServiceProvider();

var useCase = serviceProvider.GetRequiredService<RunTranslationUseCase>();
var speechService = serviceProvider.GetRequiredService<ISpeechService>();

// here you can choose your targetLanguage, I defaulted it as en for English, but you can use it as pt for Portuguese, es for Spanish, and so on...
var result = await useCase.ExecuteAsync("en");

Console.WriteLine($"Original Text: {result.OriginalText}");
Console.WriteLine($"Translated Text: {result.TranslatedText}");

var notifyInfo = new ProcessStartInfo
{
    FileName = "notify-send",
    UseShellExecute = false,
    CreateNoWindow = true
};
notifyInfo.ArgumentList.Add("WaylandLens");
notifyInfo.ArgumentList.Add(result.TranslatedText);
Process.Start(notifyInfo);

if (!silent)
{
    await speechService.SpeakAsync(result.OriginalText, "ru");
}