using Kreta.Shared.Models;
using Kreta.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Kreata.Backend.Repos
{
    public class RepositoryBase<TDbContext, TEntity> : IRepositoryBase<TEntity>
        where TDbContext : DbContext
        where TEntity : class, IDbEntity<TEntity>, new()

    {
        private readonly IDbContextFactory<TDbContext> _dbContextFactory;
        private readonly TDbContext _dbContext;
        private DbSet<TEntity>? _dbSet;

        public RepositoryBase(IDbContextFactory<TDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
            TDbContext dbContext = _dbContextFactory.CreateDbContext();
            _dbContext = dbContext;
            _dbSet = dbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> FindAll()
        {
            if (_dbSet is null)
            {
                return Enumerable.Empty<TEntity>().AsQueryable().AsNoTracking();
            }
            return _dbSet.AsNoTracking();
        }
        public TEntity GetById(Guid id)
        {
            if (_dbSet is null)
            {
                return new TEntity();
            }
            return _dbSet.FirstOrDefault(entity => entity.Id == id) ?? new TEntity();
        }

        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression)
        {
            if (_dbSet is null)
            {
                return Enumerable.Empty<TEntity>().AsQueryable().AsNoTracking();
            }
            return _dbSet.Where(expression).AsNoTracking();
        }

        public async Task<ControllerResponse> UpdateAsync(TEntity entity)
        {
            ControllerResponse response = new ControllerResponse();
            try
            {
                _dbContext.ChangeTracker.Clear();
                _dbContext.Entry(entity).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                response.AppendNewError(e.Message);
                response.AppendNewError($"{nameof(RepositoryBase<TDbContext, TEntity>)} osztály, {nameof(UpdateAsync)} metódusban hiba keletkezett");
                response.AppendNewError($"{entity} frissítése nem sikerült!");

            }
            return response;
        }

        public async Task<ControllerResponse> DeleteAsync(Guid id)
        {
            ControllerResponse response = new ControllerResponse();
            TEntity studentToDelete = GetById(id);
            if (!studentToDelete.HasId)
            {
                response.AppendNewError($"{id} idével rendelkező entitás nem található!");
                response.AppendNewError("Az entitás törlése nem sikerült!");
            }
            else
            {
                try
                {
                    _dbContext.ChangeTracker.Clear();
                    _dbContext.Entry(studentToDelete).State = EntityState.Deleted;
                    await _dbContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    response.AppendNewError(e.Message);
                    response.AppendNewError($"{nameof(RepositoryBase<TDbContext, TEntity>)} osztály, {nameof(DeleteAsync)} metódusban hiba keletkezett");
                    response.AppendNewError($"Az entitás id:{id}");
                    response.AppendNewError($"Az entitás törlése nem sikerült!");
                }
            }
            return response;
        }


        public Task<ControllerResponse> InsertAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
