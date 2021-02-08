using System.Collections.Generic;
using System.Threading.Tasks;
using ConsimpleTestTask.WebApp.DTO;
using ConsimpleTestTask.WebApp.Models;
using IRO.Mvc.CoolSwagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Controllers
{
    [ApiController]
    [Route(AppSettings.ApiPath + "/test_model")]
    public class TestController : ControllerBase
    {
        readonly DatabaseContext _db;
        readonly DbSet<TestModel> _dbSet;

        public TestController(DatabaseContext db)
        {
            _db = db;
            _dbSet = db.TestModels;
        }

        [HttpGet("get")]
        public async Task<TestModel> Get([FromBody] IdRequest req)
        {
            var model = await _dbSet.FirstAsync(r => r.Id == req.Id);
            return model;
        }

        [HttpGet("getAll")]
        public async Task<IList<TestModel>> GetAll()
        {
            var models = await _dbSet.ToListAsync();
            return models;
        }

        [HttpPut("insert")]
        public async Task<TestModel> Insert([FromBody] EntityRequest<TestModel> req)
        {
            var entry = await _dbSet.AddAsync(req.Entity);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        [HttpPatch("update")]
        public async Task<TestModel> Update([FromBody] EntityRequest<TestModel> req)
        {
            var entry = _dbSet.Update(req.Entity);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] IdRequest req)
        {
            var modelToDel = new TestModel() { Id = req.Id };
            _dbSet.Attach(modelToDel);
            _dbSet.Remove(modelToDel);
            await _db.SaveChangesAsync();
        }


    }
}
