using DictionaryOnline.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DictionaryOnline.Data
{
    public class DictionaryDbContext:DbContext
    {
        public DictionaryDbContext(DbContextOptions<DictionaryDbContext> options)
       : base(options)
        {
        }
        // DbSet đại diện cho bảng DictionaryWords trong CSDL
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Dictionary> Dictionaries { get; set; }
        public DbSet<Word> Words { get; set; }
        public DbSet<Translation> Translations { get; set; }
        public DbSet<SearchHistory> SearchHistories { get; set; }
        public DbSet<UserPreference> UserPreferences { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Thiết lập unique cho cột Word để tránh trùng lặp từ vựng
            modelBuilder.Entity<Word>()
                .HasIndex(w => w.DictionaryId)
                .IsUnique();
        }
    }
}
