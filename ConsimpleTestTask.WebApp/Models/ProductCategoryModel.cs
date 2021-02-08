using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsimpleTestTask.WebApp.Models
{
    public class ProductCategoryModel
    {
        public int Id { get; set; }

        [StringLength(30)]
        [Required]
        public string CategoryName { get; set; }

        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}