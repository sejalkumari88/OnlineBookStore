using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineBookStore.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public User User { get; set; } = null!;

        public int BookID { get; set; }

        [ForeignKey("BookID")]
        public Book Book { get; set; } = null!;

        public int Quantity { get; set; }
    }
}
