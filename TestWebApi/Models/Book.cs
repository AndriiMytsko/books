using System.ComponentModel.DataAnnotations;

namespace TestWebApi.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(256)]
        public string Title { get; set; }

        [Required]
        [MaxLength(256)]
        public string Author { get; set; }
    }
}
