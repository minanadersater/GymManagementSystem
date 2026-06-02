using GymManagementSystem.DAL.Context;
using GymManagementSystem.DAL.Entities;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity, new()
    {
        private readonly GymDbcontext dbContext;
        public GenericRepository(GymDbcontext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(TEntity item)
        {
            dbContext.Set<TEntity>().Add(item);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().AnyAsync(predicate,ct);
        }

        public async Task<int> CompleteAsync()
        {
            return await dbContext.SaveChangesAsync();

        }

        public async void Delete(int id)
        {
            var item = await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id);
            if (item != null) {
                dbContext.Set<TEntity>().Remove(item);
            }
        }

        public Task<TEntity?> FirestOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool isTracked = false,
            CancellationToken ct = default)
        {
            var Items = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return Items.FirstOrDefaultAsync(predicate, ct);
        }

        public async Task<IEnumerable<TEntity>> GetAll(bool isTracked, CancellationToken ct = default)
        {
           var Item = isTracked ? dbContext.Set<TEntity>() : dbContext.Set<TEntity>().AsNoTracking();
            return await Item.ToListAsync();
        }

        public async Task<TEntity?> GetById(int id, CancellationToken ct = default)
        {
            var Item = await dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
            return Item;
        }

        public void Update(TEntity item)
        {
            dbContext.Set<TEntity>().Add(item);
        }
    }
}
