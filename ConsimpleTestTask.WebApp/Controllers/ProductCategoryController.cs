using ConsimpleTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/productCategories")]
    public class ProductCategoryController : BaseCrudController<ProductModel>
    {
        public ProductCategoryController(DatabaseContext db) : base(db)
        {
        }
    }
}