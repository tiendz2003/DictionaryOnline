using DictionaryOnline.ViewModel;

namespace DictionaryOnline.Services
{
    public interface ITranslationService
    {
      Task<TranslationResult> TranslateAsync(string text, string fromLanguage, string toLanguage);

    }
}
