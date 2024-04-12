using System.ComponentModel.DataAnnotations;

namespace SmartWatchesStore.ViewModels
{
    public class ProductFormViewModel
    {
        public long Id { get; set; }

        [Required, StringLength(250)]
        public string Name { get; set; }
        
        public string Color { get; set; }

        public decimal Price { get; set; }

        [Required, StringLength(2500)]
        public string Description { get; set; }

        [Display(Name = "Select poster...")]
        public byte[]? Poster { get; set; }
    }
}
