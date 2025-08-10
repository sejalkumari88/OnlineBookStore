using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineBookStore.Models
{
    public class Review
    {
        public int ReviewID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public int BookID { get; set; }
        public Book Book { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime ReviewDate { get; set; } = DateTime.Now;
    }
}
