using Kreta.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kreata.Backend.Repos
{
    public class RepositoryBase<TDbContext, TEntity> : IRepositoryBase<TEntity>
        where TDbContext : DbContext
        where TEntity : class, new()

    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private DbSet<TEntity>? _dbSet;

        public IQueryable<TEntity> FindAll()
        {
            if (_dbSet is null)
            {
                return Enumerable.Empty<TEntity>().AsQueryable().AsNoTracking();
            }
            return _dbSet.AsNoTracking();
        }

        public RepositoryBase(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            TDbContext dbContext = _dbContextFactory.CreateDbContext();
            _dbSet = dbContext.Set<TEntity>();
        }

        public Task<ControllerResponse> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public TEntity GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ControllerResponse> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task<ControllerResponse> UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
