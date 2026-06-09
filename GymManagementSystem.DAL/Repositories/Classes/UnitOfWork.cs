using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbcontext dbContext;
        private readonly Dictionary<string, object> _Repos = [];
        public UnitOfWork(GymDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> CompleteAsync()
        {
           return await dbContext.SaveChangesAsync();
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var TypeName = typeof(TEntity).Name;

            if (_Repos.TryGetValue(TypeName, out object OldRepository))
                return(IGenericRepository<TEntity>)OldRepository;

            var NewRepository = new GenericRepository<TEntity>(dbContext);
            _Repos[TypeName] = NewRepository;
            return NewRepository;
        }
    }
}
