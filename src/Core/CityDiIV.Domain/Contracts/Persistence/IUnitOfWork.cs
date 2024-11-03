using CityDiIV.Domain.Entities.Common;

namespace CityDiIV.Domain.Contracts.Persistence;
public interface IUnitOfWork
{
    public IRepository<TEntity> GetRepository<TEntity>(bool onlyRead = false) where TEntity : EntityBase;
    Task<int> SaveChanges();
}
