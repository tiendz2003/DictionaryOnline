using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace DictionaryOnline.Models
{
    public class Dictionary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SourceLanguage { get; set; }
        public string TargetLanguage { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<Word> Words { get; set; }
    }

    public class Word
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Pronunciation { get; set; }
        public string PartOfSpeech { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
     

        public int DictionaryId { get; set; }
        [ForeignKey("DictionaryId")]
        public virtual Dictionary Dictionary { get; set; }

        public virtual ICollection<Translation> Translations { get; set; }
    }

    public class Translation
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string Example { get; set; }
        public string Notes { get; set; }

        public int WordId { get; set; }
        public virtual Word Word { get; set; }
    }
}
