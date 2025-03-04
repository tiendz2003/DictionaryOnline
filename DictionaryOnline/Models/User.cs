using System.Data;

namespace DictionaryOnline.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public virtual ICollection<SearchHistory> SearchHistories { get; set; }
        public virtual UserPreference Preference { get; set; }
    }

    public class UserPreference
    {
        public int Id { get; set; }
        public string DefaultSourceLanguage { get; set; }
        public string DefaultTargetLanguage { get; set; }
        public bool SaveSearchHistory { get; set; }
        public string Theme { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class SearchHistory
    {
        public int Id { get; set; }
        public string SearchTerm { get; set; }
        public string FromLanguage { get; set; }
        public string ToLanguage { get; set; }
        public DateTime SearchDate { get; set; }

        public int UserId { get; set; }
        public virtual User User { get; set; }
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

}
