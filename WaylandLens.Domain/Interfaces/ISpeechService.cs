namespace WaylandLens.Domain.Interfaces;

public interface ISpeechService
{
   Task SpeakAsync(string text, string language); 
}