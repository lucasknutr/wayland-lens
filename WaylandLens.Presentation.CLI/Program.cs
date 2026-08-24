using Microsoft.Extensions.DependencyInjection;
using WaylandLens.Application.UseCases;
using WaylandLens.Domain.Interfaces;
using WaylandLens.Infrastructure.Ocr;
using WaylandLens.Infrastructure.Translation;
using WaylandLens.Infrastructure.Wayland;

var services = new ServiceCollection();

services.AddTransient<IScreenCaptureService, GrimSlurpCaptureService>();
services.AddTransient<IOcrService, TesseractCliOcrService>();
services.AddTransient<RunTranslationUseCase>();

services.AddHttpClient<ITranslationService, GoogleTranslateApiService>();

var serviceProvider = services.BuildServiceProvider();

var useCase = serviceProvider.GetRequiredService<RunTranslationUseCase>();

var result = await useCase.ExecuteAsync("pt");

Console.WriteLine($"Translation: {result}");