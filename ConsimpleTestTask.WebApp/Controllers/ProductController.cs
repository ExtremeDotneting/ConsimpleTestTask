using ConsimpleTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/products")]
    public class ProductController : BaseCrudController<ProductModel>
    {
        public ProductController(DatabaseContext db) : base(db)
        {
        }
    }
}