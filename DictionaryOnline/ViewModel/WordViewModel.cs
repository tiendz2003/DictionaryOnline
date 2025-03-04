using System.ComponentModel.DataAnnotations;

namespace DictionaryOnline.ViewModel
{
    public class WordViewModel
    {   public int Id { get; set; }
        [Required(ErrorMessage = "Từ vựng là bắt buộc")]
        public string Text { get; set; }

        public string Pronunciation { get; set; }

        [Required(ErrorMessage = "Loại từ là bắt buộc")]
        public string PartOfSpeech { get; set; }

        [Required(ErrorMessage = "Từ điển là bắt buộc")]
        public int DictionaryId { get; set; }

        // Dữ liệu bản dịch
        [Required(ErrorMessage = "Bản dịch không được để trống")]
        public string Translation { get; set; }

        public string Example { get; set; }
        public string Notes { get; set; }
    }
}
