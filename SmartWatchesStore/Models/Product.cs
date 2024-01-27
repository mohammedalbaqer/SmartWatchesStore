using System.ComponentModel.DataAnnotations;

namespace SmartWatchesStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Name { get; set; }

        [Required, MaxLength(250)]
        public string Color { get; set; }

        [Required, MaxLength(2500)]

        public double Price { get; set; }

        public string Description { get; set; }

        [Required]
        public byte[] Poster { get; set; }

    }
}
