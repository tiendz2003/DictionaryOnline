using DictionaryOnline.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DictionaryOnline.ViewModel
{
    public class GoogleTranslateResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public List<Translation> Translations { get; set; }
    }
    public class Translation
    {
        public string TranslatedText { get; set; }
        public string DetectedSourceLanguage { get; set; }
    }
}
