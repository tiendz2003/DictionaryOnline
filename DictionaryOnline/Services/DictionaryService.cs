using DictionaryOnline.Data;
using DictionaryOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DictionaryOnline.Services
{
    public class DictionaryService
    {
        private readonly DictionaryDbContext _context;

        public DictionaryService(DictionaryDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Dictionary>> GetAllDictionariesAsync()
        {
            return await _context.Dictionaries.ToListAsync();
        }
        public async Task<Dictionary> GetDictionaryByIdAsync(int id)
        {
            return await _context.Dictionaries.FindAsync(id);
        }
        public async Task<IEnumerable<Word>> SearchWordAsync(string term, int dictionaryId)
        {
            return await _context.Words
                .Include(w => w.Translations)
                .Where(w => w.Text.Contains(term) && w.DictionaryId == dictionaryId)
                .ToListAsync();
        }
        public async Task<Word> GetWordByIdAsync(int id)
        {
            return await _context.Words
                .Include(w => w.Translations)
                .FirstOrDefaultAsync(w => w.Id == id);
        }
    }
}
