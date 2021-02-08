using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsimpleTestTask.WebApp.DTO;
using ConsimpleTestTask.WebApp.Models;
using IRO.Mvc.CoolSwagger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConsimpleTestTask.WebApp.Controllers
{
    public abstract class BaseCrudController<TEntity> : ControllerBase
        where TEntity : class, IBaseModel
    {
        readonly DatabaseContext _db;
        readonly DbSet<TEntity> _dbSet;

        protected BaseCrudController(DatabaseContext db)
        {
            _db = db;
            _dbSet = db.Set<TEntity>();
        }

        [HttpGet("get")]
        public virtual async Task<TEntity> Get([FromQuery] int id)
        {
            var model = await _dbSet.FirstAsync(r => r.Id == id);
            return model;
        }

        [HttpGet("getAll")]
        public virtual async Task<IList<TEntity>> GetAll()
        {
            
            var models = await _dbSet.ToListAsync();
            return models;
        }

        [HttpPut("insert")]
        public async Task<TEntity> Insert([FromBody] EntityRequest<TEntity> req)
        {
            var entry = await _dbSet.AddAsync(req.Entity);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        [HttpPatch("update")]
        public async Task<TEntity> Update([FromBody] EntityRequest<TEntity> req)
        {
            var entry = _dbSet.Update(req.Entity);
            await _db.SaveChangesAsync();
            return entry.Entity;
        }

        [HttpDelete("delete")]
        public async Task Delete([FromBody] IdRequest req)
        {
            var modelToDel =Activator.CreateInstance<TEntity>();
            modelToDel.Id = req.Id;
            _dbSet.Attach(modelToDel);
            _dbSet.Remove(modelToDel);
            await _db.SaveChangesAsync();
        }


    }

}
