using Microsoft.EntityFrameworkCore;

namespace Kreata.Backend.Repos
{
    public class RepositoryBase<TDbContext, TEntity> : IRepositoryBase<TEntity>
        where TDbContext : DbContext
        where TEntity : class, new()

    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private DbSet<TEntity>? _dbSet;

        public RepositoryBase(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            TDbContext dbContext = _dbContextFactory.CreateDbContext();
            _dbSet = dbContext.Set<TEntity>();
        }
    }
}
