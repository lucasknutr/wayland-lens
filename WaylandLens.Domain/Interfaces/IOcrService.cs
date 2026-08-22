using WaylandLens.Domain.Entities;

namespace WaylandLens.Domain.Interfaces;

public interface IOcrService
{
   Task<OcrResult> GetOcrResultAsync(ImageCapture capture);
}