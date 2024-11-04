using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ReviewBooks.Models
{
    public class BookReview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReviewText { get; set; }

        public DateTime ReviewDate { get; set; }

        // Llave foránea a la clase `Book`
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        // Llave foránea a la clase `User`
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        [Range(1, 5, ErrorMessage = "La calificación debe ser entre 1 y 5.")]
        public int Qualifying { get; set; }
    }
}
