using WaylandLens.Domain.Entities;

namespace WaylandLens.Domain.Interfaces;

public interface ITranslationService
{
   Task<string> TranslateAsync(string text, string targetLanguage);  
}