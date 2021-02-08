using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConsimpleTestTask.WebApp.DTO;
using ConsimpleTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/users")]
    public class UserController : BaseCrudController<UserModel>
    {
        readonly DatabaseContext _db;

        public UserController(DatabaseContext db) : base(db)
        {
            _db = db;
        }


        [HttpGet("get")]
        public override async Task<UserModel> Get([FromQuery] int id)
        {
            var model = await _db.Users
                .Include(r => r.Purchases)
                .ThenInclude(r => r.PurchasePositions)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.ProductCategory)
                .FirstAsync(r => r.Id == id);
            return model;
        }

        [HttpGet("getAll")]
        public override async Task<IList<UserModel>> GetAll()
        {
            var models = await _db.Users
                .Include(r => r.Purchases)
                .ThenInclude(r => r.PurchasePositions)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.ProductCategory)
                .ToListAsync();
            return models;
        }

        [HttpGet("getByBirthday")]
        public async Task<IList<UserModel>> GetByBirthday([FromQuery]int month, [FromQuery] int day)
        {
            var birthdayUsers = await _db.Users
                .Where(r => r.BirthDate.Day == day && r.BirthDate.Month == month)
                .ToListAsync();
            return birthdayUsers;
        }

        [HttpGet("getLastByers")]
        public async Task<IList<UserModel>> GetLastByers([FromQuery] int days)
        {
            //Maybe better just write last buy date to user table.
            var minDate = DateTime.UtcNow.Date - TimeSpan.FromDays(days);
            var lastBuyers = await _db.Users
                .Where(r => r.Purchases.Any(pr => pr.Date > minDate))
                .ToListAsync();
            return lastBuyers;
        }

        [HttpGet("getPurchasesStats")]
        public async Task<IList<PurchasesStatsDto>> GetPurchasesStats([FromQuery] int id)
        {
            var statsByCategoryIdDict = new Dictionary<int, PurchasesStatsDto>();

            var model = await _db.Users
                .Include(r => r.Purchases)
                .ThenInclude(r => r.PurchasePositions)
                .ThenInclude(r => r.Product)
                .ThenInclude(r => r.ProductCategory)
                .FirstAsync(r => r.Id == id);

            foreach (var purchase in model.Purchases)
            {
                foreach (var position in purchase.PurchasePositions)
                {
                    var category = position.Product.ProductCategory;
                    if (!statsByCategoryIdDict.TryGetValue(category.Id, out var stats))
                    {
                        stats = new PurchasesStatsDto()
                        {
                            CategoryId = category.Id,
                            CategoryName= category.CategoryName
                        };
                        statsByCategoryIdDict[category.Id] = stats;
                    }
                    stats.BuyCount++;
                }
            }

            var resList = statsByCategoryIdDict
                .AsEnumerable()
                .Select(r => r.Value)
                .ToList();
            return resList;
        }
    }
}