using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Application.UseCases;

public class RunTranslationUseCase(
    ITranslationService translationService,
    IOcrService ocrService,
    IScreenCaptureService screenCaptureService)
{
    private readonly IScreenCaptureService _screenCaptureService = screenCaptureService;
    private readonly ITranslationService _translationService = translationService;
    private readonly IOcrService _ocrService = ocrService;
}