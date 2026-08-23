using System.Diagnostics;
using WaylandLens.Domain.Entities;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Ocr;

public class TesseractCliOcrService : IOcrService
{
    public async Task<OcrResult> GetOcrResultAsync(ImageCapture capture)
    {
        // get a temporary file location inside my /tmp directory
        string filePath = Path.GetTempFileName();

        await File.WriteAllBytesAsync(filePath, capture.Data);

        var processInfo = new ProcessStartInfo
        {
            FileName = "tesseract",
            Arguments = $"\"\\\"{filePath}\\\" stdout -l eng+rus",
            RedirectStandardOutput = true,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        using var process = new Process();
        process.StartInfo = processInfo;

        process.Start();

        var extractedText = await process.StandardOutput.ReadToEndAsync();

        await process.WaitForExitAsync();

        File.Delete(filePath);

        return new OcrResult{ RawText = extractedText };
}
}