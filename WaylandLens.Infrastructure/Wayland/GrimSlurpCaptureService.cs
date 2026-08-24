using System.Diagnostics;
using WaylandLens.Domain.Entities;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Wayland;

public class GrimSlurpCaptureService : IScreenCaptureService
{
   public async Task<ImageCapture> CaptureScreenAsync()
   {
      var processInfo = new ProcessStartInfo
      {
         FileName = "bash",
         UseShellExecute = false,
         RedirectStandardOutput = true,
         CreateNoWindow = true,
      };
      
      processInfo.ArgumentList.Add("-c");
      processInfo.ArgumentList.Add("grim -g \"$(slurp)\" -");

      using var process = new Process();
      process.StartInfo = processInfo;
      
      process.Start();
      
      using var memoryStream = new MemoryStream();

      await process.StandardOutput.BaseStream.CopyToAsync(memoryStream);

      await process.WaitForExitAsync();
      
      return new ImageCapture { Data = memoryStream.ToArray() };
   }
}