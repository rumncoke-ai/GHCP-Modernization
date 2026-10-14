using System.ComponentModel.DataAnnotations;

namespace LegacyShop.Web.ViewModels
{
    public class ProductEditViewModel
    {
        public int ProductId { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [StringLength(1000)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(typeof(decimal), "0", "999999")]
        public decimal Price { get; set; }

        [Display(Name = "Stock quantity")]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Category")]
        [Range(1, int.MaxValue, ErrorMessage = "Select a category.")]
        public int CategoryId { get; set; }
    }
}
