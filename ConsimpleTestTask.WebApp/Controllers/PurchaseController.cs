using ConsimpleTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/purchases")]
    public class PurchaseController : BaseCrudController<ProductModel>
    {
        public PurchaseController(DatabaseContext db) : base(db)
        {
        }
    }
}