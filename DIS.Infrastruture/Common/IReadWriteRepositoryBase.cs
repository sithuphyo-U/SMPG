using DIS.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Common
{
    public interface IReadWriteRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        TEntity? Get(int id);
        List<TEntity> Get();
        List<TEntity> GetReport();
        PagedResult<TEntity> GetPagedResults(QueryOptions<TEntity> option);
        CommandResult<TEntity> Save(TEntity entity);
        CommandResult<TEntity> Remove(TEntity entity);
    }
}
