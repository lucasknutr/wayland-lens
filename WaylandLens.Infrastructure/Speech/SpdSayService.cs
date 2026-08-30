using System.Diagnostics;
using WaylandLens.Domain.Interfaces;

namespace WaylandLens.Infrastructure.Speech;

public class SpdSayService : ISpeechService
{
   public async Task SpeakAsync(string text, string language)
   {
      var processInfo = new ProcessStartInfo
      {
         FileName = "spd-say",
         UseShellExecute = false,
         CreateNoWindow = true
      };

      processInfo.ArgumentList.Add("-l");
      
      processInfo.ArgumentList.Add(language);
      processInfo.ArgumentList.Add(text);
      
      using var process = Process.Start(processInfo);
      await process.WaitForExitAsync();
   }
}