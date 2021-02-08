using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsimpleTestTask.WebApp.DTO;
using ConsimpleTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/common")]
    public class CommonController : ControllerBase
    {
        readonly DatabaseContext _db;

        public CommonController(DatabaseContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Just for tests.
        /// </summary>
        /// <returns></returns>
        [HttpGet("seedDatabase")]
        public async Task<string> SeedDatabase()
        {
            await UpsertCategories(new[]
            {
                "Електроника",
                "Мебель",
                "Для огорода",
                "Ремонт"
            });
            await _db.SaveChangesAsync();
            await UpsertProducts(new (string Category, string ProductName, string VendorCode, decimal Price)[]
            {
                ("Електроника", "Какой-то ноутбук", "Арт.121663", 23999),
                ("Електроника", "Какой-то комп", "Арт.581148", 19999),
                ("Електроника", "Какой-то холодильник", "Арт.654812", 14999),
                ("Мебель","Диван", "Арт.094185", 14999),
                ("Мебель","Стол", "Арт.094185", 9999),
                ("Мебель","Кресло", "Арт.832791", 7999),
                ("Для огорода","Лопата", "Арт.662331", 120),
                ("Для огорода","Грабли", "Арт.662331", 120),
                ("Ремонт","Дрель", "Арт.662331", 5999),
                ("Ремонт","Шпаклевка", "Арт.662331", 500),
            });
            await _db.SaveChangesAsync();
            await UpsertUsers(new (string UserName, DateTime BirthAt, DateTime RegisteredAt)[]
            {
                ("Саня", new DateTime(1999, 1, 10), new DateTime(2020, 11, 14)),
                ("Витя", new DateTime(1999, 1, 10), new DateTime(2020, 11, 14)),
            });
            await _db.SaveChangesAsync();
            await MakeRandomPurchases();
            await _db.SaveChangesAsync();
            return "Database seed success.";
        }

        async Task MakeRandomPurchases()
        {
            var rd = new Random();
            var users = await _db.Users.ToListAsync();
            var products = await _db.Products.ToListAsync();
            foreach (var user in users)
            {
                var product1 = products[rd.Next(products.Count)];
                var product2 = products[rd.Next(products.Count)];
                var newPurchase = new PurchaseModel()
                {
                    UserId=user.Id,
                    PurchasePositions = new List<PurchasePositionModel>()
                    {
                        new PurchasePositionModel()
                        {
                            ProductId = product1.Id,
                            Product=product1,
                            ProductsMultiplier = rd.Next(5)
                        },
                        new PurchasePositionModel()
                        {
                            ProductId = product2.Id,
                            Product=product2,
                            ProductsMultiplier = rd.Next(5)
                        }
                    },
                    Date=DateTime.UtcNow
                };
                newPurchase.RecalculateTotalCost();
                await _db.Purchases.AddAsync(newPurchase);

            }
        }

        async Task UpsertProducts(
            IEnumerable<(string Category, string ProductName, string VendorCode, decimal Price)> products
        )
        {
            foreach (var pr in products)
            {
                var categoryModel = await _db.ProductsCategories.FirstAsync(r => r.CategoryName == pr.Category);
                var newModel = new ProductModel()
                {
                    ProductCategoryId = categoryModel.Id,
                    Price = pr.Price,
                    Title = pr.ProductName,
                    VendorCode = pr.VendorCode
                };
                await _db.Products.Upsert(newModel)
                    .On(r => r.Title)
                    .NoUpdate()
                    .RunAsync();
            }
        }

        async Task UpsertUsers(IEnumerable<(string UserName, DateTime BirthAt, DateTime RegisteredAt)> users)
        {
            foreach (var user in users)
            {
                var newModel = new UserModel()
                {
                    FullName = user.UserName,
                    BirthDate = user.BirthAt,
                    RegistrationDate = user.RegisteredAt
                };
                await _db.Users.Upsert(newModel)
                    .On(r => r.FullName)
                    .NoUpdate()
                    .RunAsync();
            }
        }

        async Task UpsertCategories(IEnumerable<string> categoryNames)
        {
            foreach (var categoryName in categoryNames)
            {
                var newModel = new ProductCategoryModel()
                {
                    CategoryName = categoryName
                };
                await _db.ProductsCategories.Upsert(newModel)
                    .On(r => r.CategoryName)
                    .NoUpdate()
                    .RunAsync();
            }
        }
    }
}