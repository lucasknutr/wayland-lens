using Microsoft.Extensions.DependencyInjection;
using WaylandLens.Application.UseCases;
using WaylandLens.Domain.Interfaces;
using WaylandLens.Infrastructure.Ocr;
using WaylandLens.Infrastructure.Translation;
using WaylandLens.Infrastructure.Wayland;
using WaylandLens.Infrastructure.Speech;

var services = new ServiceCollection();

services.AddTransient<IScreenCaptureService, GrimSlurpCaptureService>();
services.AddTransient<IOcrService, TesseractCliOcrService>();
services.AddTransient<RunTranslationUseCase>();
services.AddTransient<ISpeechService, SpdSayService>();

services.AddHttpClient<ITranslationService, GoogleTranslateApiService>();

var serviceProvider = services.BuildServiceProvider();

var useCase = serviceProvider.GetRequiredService<RunTranslationUseCase>();
var speechService = serviceProvider.GetRequiredService<ISpeechService>();

// here you can choose your targetLanguage, I defaulted it as en for English, but you can use it as pt for Portuguese, es for Spanish, and so on...
var result = await useCase.ExecuteAsync("en");

Console.WriteLine($"Original Text: {result.OriginalText}");
Console.WriteLine($"Translated Text: {result.TranslatedText}");
Console.WriteLine("Do you wish to hear it? Y/N");

var answer = Console.ReadLine();
if (answer == "Y" || answer == "y")
{
    await speechService.SpeakAsync(result.OriginalText, "ru");
}