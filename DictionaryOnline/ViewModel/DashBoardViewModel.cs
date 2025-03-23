namespace DictionaryOnline.ViewModel
{
    public class DashboardViewModel
    {
        public int TotalUsers { get; set; } // Tổng số người dùng
        public int TotalDictionaries { get; set; } // Tổng số từ điển
        public int TotalWords { get; set; } // Tổng số từ trong hệ thống
        public List<SearchHistoryViewModel>? RecentSearches { get; set; } // Danh sách 10 tìm kiếm gần nhất
     
    }
    public class SearchHistoryViewModel
    {
        public string? SearchTerm { get; set; } // Từ khóa tìm kiếm
        public DateTime SearchDate { get; set; } // Ngày tìm kiếm
        public string? UserEmail { get; set; } // Email người dùng đã tìm kiếm (nếu có)
    }
}
