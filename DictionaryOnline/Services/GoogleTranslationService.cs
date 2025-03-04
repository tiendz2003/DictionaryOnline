using DictionaryOnline.ViewModel;

namespace DictionaryOnline.Services
{
    public class GoogleTranslationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public GoogleTranslationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GoogleTranslate:ApiKey"];
        }
        public async Task<TranslationResult> TranslateAsync(string text, string fromLanguage, string toLanguage)
        {
            var url = $"https://translation.googleapis.com/language/translate/v2?key={_apiKey}";

            var content = new
            {
                q = text,
                source = fromLanguage,
                target = toLanguage,
                format = "text"
            };

            var response = await _httpClient.PostAsJsonAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GoogleTranslateResponse>();
                return new TranslationResult
                {
                    OriginalText = text,
                    TranslatedText = result.Data.Translations[0].TranslatedText,
                    SourceLanguage = fromLanguage,
                    TargetLanguage = toLanguage
                };
            }

            throw new Exception("Failed to connect to Google Translate API");
        }
    }
}
