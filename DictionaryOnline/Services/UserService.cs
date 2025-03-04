using DictionaryOnline.Data;
using DictionaryOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DictionaryOnline.Services
{
    public class UserService
    {
        private readonly DictionaryDbContext _context;

        public UserService(DictionaryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<SearchHistory>> GetUserSearchHistoryAsync(int userId)
        {
            return await _context.SearchHistories
                .Where(h => h.UserId == userId)
                .OrderByDescending(h => h.SearchDate)
                .ToListAsync();

        }
        public async Task<UserPreference> GetUserPreferenceAsync(int userId)
        {
            return await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == userId);
        }
        public async Task UpdateUserPreferenceAsync(UserPreference preference)
        {
            var existingPreference = await _context.UserPreferences
                .FirstOrDefaultAsync(p => p.UserId == preference.UserId);

            if (existingPreference != null)
            {
                existingPreference.DefaultSourceLanguage = preference.DefaultSourceLanguage;
                existingPreference.DefaultTargetLanguage = preference.DefaultTargetLanguage;
                existingPreference.SaveSearchHistory = preference.SaveSearchHistory;
                existingPreference.Theme = preference.Theme;
            }
            else
            {
                _context.UserPreferences.Add(preference);
            }

            await _context.SaveChangesAsync();
        }
    }
}
