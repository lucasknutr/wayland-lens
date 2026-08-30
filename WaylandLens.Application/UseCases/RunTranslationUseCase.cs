using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Application.UseCases;

public class RunTranslationUseCase(
    ITranslationService translationService,
    IOcrService ocrService,
    IScreenCaptureService screenCaptureService)
{
    public async Task<string> ExecuteAsync(string targetLanguage)
    {
        var image = await screenCaptureService.CaptureScreenAsync();
        var textResult = await ocrService.GetOcrResultAsync(image);
        var translatedText = await translationService.TranslateAsync(textResult.RawText, targetLanguage);
        
        // return translatedText;
        // return RawText + translatedText
        s.Add("Original Text: $'textResult.RawText'");
        s.Add(translatedText);
        return s;
    }
    public record TranslationResult(string OriginalText, string TranslatedText)
    {
        OriginalText = this.originalText
        return OriginalText + TranslatedText;
    }
}