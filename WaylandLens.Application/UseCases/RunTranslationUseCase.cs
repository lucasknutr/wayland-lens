using WaylandLens.Domain.Interfaces;
using WaylandLens.Domain.Entities;

namespace WaylandLens.Application.UseCases;

public class RunTranslationUseCase(
    ITranslationService translationService,
    IOcrService ocrService,
    IScreenCaptureService screenCaptureService)
{
    public async Task<TranslationResult> ExecuteAsync(string targetLanguage)
    {
        var image = await screenCaptureService.CaptureScreenAsync();
        var textResult = await ocrService.GetOcrResultAsync(image);
        var translatedText = await translationService.TranslateAsync(textResult.RawText, targetLanguage);

        return new TranslationResult
        {
            TranslatedText = translatedText,
            OriginalText = textResult.RawText
        };
    }
}