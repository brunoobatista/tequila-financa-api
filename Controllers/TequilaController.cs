using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Tequila.Models.Interfaces;
using Tequila.Repositories.Interfaces;

namespace Tequila.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public abstract class TequilaController<TEntity, TRepository> : ControllerBase
        where TEntity : class, IEntity
        where TRepository : IRepository<TEntity>
    {
        private readonly TRepository _repository;

        public TequilaController(TRepository repository)
        {
            _repository = repository;
        }
        
        // GET: api/[controller]
        [HttpGet]
        public ActionResult<List<TEntity>> Get()
        {
            return _repository.GetAll();
        }

        // GET: api/[controller]/5
        [HttpGet("{id}", Name = nameof(Get))]
        public ActionResult<TEntity> Get(int id)
        {
            var entity =  _repository.Get(id);
            if (entity == null)
            {
                return NotFound(typeof(TEntity).ToString() + " não encontrado");
            }
            return entity;
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public  IActionResult Put(long id, TEntity entity)
        {
            if (id != entity.Id)
            {
                return BadRequest();
            }
            _repository.Update(entity);
            return NoContent();
        }

        // POST: api/[controller]
        [HttpPost]
        public ActionResult<TEntity> Post(TEntity entity)
        {
            _repository.Add(entity);
            return CreatedAtRoute("Get", new { id = entity.Id }, entity);
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public ActionResult<TEntity> Delete(int id)
        {
            var entity = _repository.Delete(id);
            if (entity == null)
            {
                return NotFound(typeof(TEntity).ToString() + " não encontrado");
            }
            return entity;
        }

    }
}