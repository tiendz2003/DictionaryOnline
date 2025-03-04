using System.ComponentModel.DataAnnotations;

namespace DictionaryOnline.ViewModel
{
    public class DictionaryViewModel
    {
        public int Id { get; set; } // ID của từ điển (dùng khi chỉnh sửa)

        [Required(ErrorMessage = "Tên từ điển không được để trống")]
        [StringLength(100, ErrorMessage = "Tên từ điển không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ngôn ngữ nguồn là bắt buộc")]
        [StringLength(50, ErrorMessage = "Ngôn ngữ nguồn không được vượt quá 50 ký tự")]
        public string SourceLanguage { get; set; }

        [Required(ErrorMessage = "Ngôn ngữ đích là bắt buộc")]
        [StringLength(50, ErrorMessage = "Ngôn ngữ đích không được vượt quá 50 ký tự")]
        public string TargetLanguage { get; set; }

        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        public bool IsActive { get; set; } // Trạng thái hoạt động của từ điển

        public DateTime UpdatedAt { get; set; } // Dùng để hiển thị thông tin cập nhật
    }
}
