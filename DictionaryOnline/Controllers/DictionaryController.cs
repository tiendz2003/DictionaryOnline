using DictionaryOnline.Data;
using DictionaryOnline.Models;
using DictionaryOnline.Services;
using DictionaryOnline.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DictionaryOnline.Controllers
{
    public class DictionaryController:Controller
    {
        private readonly DictionaryDbContext _context;
        private readonly ITranslationService _translationService;

        public DictionaryController(DictionaryDbContext context, ITranslationService translationService)
        {
            _context = context;
            _translationService = translationService;
        }
        public async Task<IActionResult> Index()
        {
            var dictionaries = await _context.Dictionaries.ToListAsync();
            return View(dictionaries);
        }
        /*  public async Task<IActionResult> Search(string term, string fromLang, string toLang)
          {
              // Tìm kiếm trong cơ sở dữ liệu trước
              var word = await _context.Words
                  .Include(w => w.Translations)
                  .FirstOrDefaultAsync(w => w.Text == term && w.Dictionary.SourceLanguage == fromLang && w.Dictionary.TargetLanguage == toLang);

              // Nếu không tìm thấy, sử dụng Google Translate API
              if (word == null)
              {
                  var translation = await _translationService.TranslateAsync(term, fromLang, toLang);

                  // Lưu vào lịch sử tìm kiếm nếu người dùng đã đăng nhập
                  if (User.Identity.IsAuthenticated)
                  {
                      var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                      var searchHistory = new SearchHistory
                      {
                          UserId = userId,
                          SearchTerm = term,
                          FromLanguage = fromLang,
                          ToLanguage = toLang,
                          SearchDate = DateTime.Now
                      };
                      _context.SearchHistories.Add(searchHistory);
                      await _context.SaveChangesAsync();
                  }

                  return View("GoogleTranslationResult", translation);
              }

              return View(word);
          }*/
        [HttpGet]
        public async Task<IActionResult> Search(string term, string fromLang, string toLang)
        {
            if (string.IsNullOrEmpty(term))
            {
                return View();
            }

            TranslationResult result;
            // Tìm kiếm trong cơ sở dữ liệu
            var word = await _context.Words
                .Include(w => w.Translations)
                .FirstOrDefaultAsync(w => w.Text == term &&
                    w.Dictionary.SourceLanguage == fromLang &&
                    w.Dictionary.TargetLanguage == toLang);

            if (word != null)
            {
                var translation = word.Translations.FirstOrDefault();
                result = new TranslationResult
                {
                    OriginalText = word.Text,
                    TranslatedText = translation?.Text,
                    SourceLanguage = fromLang,
                    TargetLanguage = toLang
                };
            }
            else
            {
                // Sử dụng Google Translate API
                var googleTranslation = await _translationService.TranslateAsync(term, fromLang, toLang);
                result = new TranslationResult
                {
                    OriginalText = term,
                    TranslatedText = googleTranslation.TranslatedText,
                    SourceLanguage = fromLang,
                    TargetLanguage = toLang
                };
            }

            // Lưu lịch sử tìm kiếm nếu user đã đăng nhập
            if (User.Identity.IsAuthenticated)
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
                var searchHistory = new SearchHistory
                {
                    UserId = userId,
                    SearchTerm = term,
                    FromLanguage = fromLang,
                    ToLanguage = toLang,
                    SearchDate = DateTime.Now
                };
                _context.SearchHistories.Add(searchHistory);
                await _context.SaveChangesAsync();
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_TranslationResult", result);
            }

            return View(result);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            // Lấy danh sách từ điển từ database
            var dictionaries = _context.Dictionaries
                .Select(d => new { d.Id, d.Name })
                .ToList();
            ViewBag.Dictionaries = dictionaries;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(WordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var word = new Word
                {
                    Text = model.Text,
                    Pronunciation = model.Pronunciation,
                    PartOfSpeech = model.PartOfSpeech,
                    DictionaryId = model.DictionaryId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var translation = new Models.Translation
                {
                    Text = model.Translation,
                    Example = model.Example,
                    Notes = model.Notes,
                    Word = word
                };

                _context.Words.Add(word);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã thêm từ mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            var dictionaries = _context.Dictionaries
            .Select(d => new { d.Id, d.Name })
            .ToList();
            ViewBag.Dictionaries = dictionaries;
            return View(model);
       
        }
        [HttpGet]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(int id)
        {
            var word = await _context.Words
                .Include(w => w.Translations)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (word == null)
            {
                return NotFound();
            }

            var model = new WordViewModel
            {
                Id = word.Id,
                Text = word.Text,
                Pronunciation = word.Pronunciation,
                PartOfSpeech = word.PartOfSpeech,
                DictionaryId = word.DictionaryId,
                Translation = word.Translations.FirstOrDefault()?.Text,
                Example = word.Translations.FirstOrDefault()?.Example,
                Notes = word.Translations.FirstOrDefault()?.Notes
            };

            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Editor")]
        public async Task<IActionResult> Edit(WordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var word = await _context.Words
                    .Include(w => w.Translations)
                    .FirstOrDefaultAsync(w => w.Id == model.Id);

                if (word == null)
                {
                    return NotFound();
                }

                word.Text = model.Text;
                word.Pronunciation = model.Pronunciation;
                word.PartOfSpeech = model.PartOfSpeech;
                word.UpdatedAt = DateTime.Now;

                var translation = word.Translations.FirstOrDefault();
                if (translation != null)
                {
                    translation.Text = model.Translation;
                    translation.Example = model.Example;
                    translation.Notes = model.Notes;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var word = await _context.Words.FindAsync(id);
            if (word == null)
            {
                return NotFound();
            }

            _context.Words.Remove(word);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
