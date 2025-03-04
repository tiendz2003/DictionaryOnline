using DictionaryOnline.Data;
using DictionaryOnline.Models;
using DictionaryOnline.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DictionaryOnline.Controllers
{
    public class AdminController:Controller
    {
        private readonly DictionaryDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(DictionaryDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
                var recentSearches = await _context.SearchHistories
                    .OrderByDescending(s => s.SearchDate)
                    .Take(10)
                    .Select(s => new SearchHistoryViewModel
                    {
                        SearchTerm = s.SearchTerm,
                        SearchDate = s.SearchDate,
                        UserEmail = s.User.Email
                    })
                    .ToListAsync();
            // Dashboard thống kê dữ liệu
            var statistics = new DashboardViewModel
            {
                TotalUsers = await _context.Users.CountAsync(),
                TotalDictionaries = await _context.Dictionaries.CountAsync(),
                TotalWords = await _context.Words.CountAsync(),
                RecentSearches = recentSearches
            };

            return View(statistics);
        }
     
        public async Task<IActionResult> ManageDictionaries()
        {
            var dictionaries = await _context.Dictionaries.ToListAsync();
            return View(dictionaries);
        }

        [HttpGet]
        public IActionResult CreateDictionary()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDictionary(DictionaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dictionary = new Dictionary
                {
                    Name = model.Name,
                    SourceLanguage = model.SourceLanguage,
                    TargetLanguage = model.TargetLanguage,
                    Description = model.Description,
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.Dictionaries.Add(dictionary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageDictionaries));
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditDictionary(int id)
        {
            var dictionary = await _context.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return NotFound();
            }

            var model = new DictionaryViewModel
            {
                Id = dictionary.Id,
                Name = dictionary.Name,
                SourceLanguage = dictionary.SourceLanguage,
                TargetLanguage = dictionary.TargetLanguage,
                Description = dictionary.Description,
                IsActive = dictionary.IsActive
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditDictionary(DictionaryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var dictionary = await _context.Dictionaries.FindAsync(model.Id);
                if (dictionary == null)
                {
                    return NotFound();
                }

                dictionary.Name = model.Name;
                dictionary.SourceLanguage = model.SourceLanguage;
                dictionary.TargetLanguage = model.TargetLanguage;
                dictionary.Description = model.Description;
                dictionary.IsActive = model.IsActive;
                dictionary.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageDictionaries));
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteDictionary(int id)
        {
            var dictionary = await _context.Dictionaries.FindAsync(id);
            if (dictionary == null)
            {
                return NotFound();
            }

            _context.Dictionaries.Remove(dictionary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageDictionaries));
        }
    }
}
