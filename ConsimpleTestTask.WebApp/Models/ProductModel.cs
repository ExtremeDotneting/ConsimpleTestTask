using System.ComponentModel.DataAnnotations;

namespace ConsimpleTestTask.WebApp.Models
{
    public class ProductModel: IBaseModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [StringLength(30)]
        [Required]
        public string VendorCode { get; set; }

        public decimal Price { get; set; }

        public int ProductCategoryId { get; set; }
        public ProductCategoryModel ProductCategory { get; set; }
    }
}