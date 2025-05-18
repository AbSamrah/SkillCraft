using RoadmapMangement.DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadmapMangement.DataAccessLayer.Interfaces
{
    public interface IRepository<TEntity> : IDisposable where TEntity : Entity
    {
        void Add(TEntity entity);
        Task<TEntity> GetById(string id);
        Task<List<TEntity>> GetAll();
        Task Update(TEntity entity);
        void Remove(string id);
    }
}
