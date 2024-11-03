using CityDiIV.Domain.Entities.Common;
using System.Collections.Concurrent;

namespace CityDiIV.Domain.Contracts.Persistence;
public interface IUnitOfWork
{
    public ConcurrentBag<object> InsertBulkBag { get; set; }

    public IRepository<TEntity> GetRepository<TEntity>(bool onlyRead = false) where TEntity : EntityBase;
    Task<int> SaveChanges();

    Task BulkInsert();
}
