using DIS.DataAccess;
using DIS.Infrastructure.Common;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess
{
    public class ReadWriteRepositoryBase<TEntity> : IReadWriteRepositoryBase<TEntity> where TEntity : BaseEntity
    {
        private readonly IDbContext _context;
        public ReadWriteRepositoryBase(IDbContext context)
        {
            _context = context;
        }
        public TEntity? Get(int id)
        {
            TEntity? entity = _context.Set<TEntity>().Where(x => x.id == id && x.deleted == false).FirstOrDefault();
            return entity;
        }
        public List<TEntity> Get()
        {
            List<TEntity> entities = _context.Set<TEntity>().Where(x => x.deleted == false).ToList();
            return entities;
        }

        public PagedResult<TEntity> GetPagedResults(QueryOptions<TEntity> option)
        {
            var results = new PagedResult<TEntity>();

            try
            {
                if (option == null)
                {
                    results.success = false;
                    results.messages.Add("Invalid query options.");
                    return results;
                }

                if (option.Page <= 0)
                    option.Page = 1;

                int skip = (option.Page - 1) * option.RecordPerPage;

                // Step 1: Build query with filter
                IQueryable<TEntity> query = _context.Set<TEntity>().Where(x => !x.deleted);

                if (option.FilterBy != null)
                    query = query.Where(option.FilterBy);

                // Step 2: Get total before paging
                results.total = query.Count();

                // Step 3: Pull filtered data into memory for in-memory sort
                var dataList = query.ToList();

                // Step 4: Apply sort
                IOrderedEnumerable<TEntity> orderedData;
                if (option.SortOrder == SortOrder.ASC)
                {
                    orderedData = dataList.OrderBy(option.SortBy[0]);
                    for (int i = 1; i < option.SortBy.Count; i++)
                        orderedData = orderedData.ThenBy(option.SortBy[i]);
                }
                else
                {
                    orderedData = dataList.OrderByDescending(option.SortBy[0]);
                    for (int i = 1; i < option.SortBy.Count; i++)
                        orderedData = orderedData.ThenByDescending(option.SortBy[i]);
                }

                // Step 5: Paging
                if (option.legth < 0)
                {
                    results.data = orderedData.ToList();
                }
                else
                {
                    results.data = orderedData.Skip(skip).Take(option.RecordPerPage).ToList();
                }
                results.success = true;
            }
            catch (Exception ex)
            {
                results.success = false;
                results.messages.Add("Something went wrong while fetching paged data.");
                results.messages.Add(ex.Message);
            }

            return results;
        }

        public List<TEntity> GetReport()
        {
            List<TEntity> entities = _context.Set<TEntity>().ToList();
            return entities;
        }

        public CommandResult<TEntity> Remove(TEntity entity)
        {
            CommandResult<TEntity> result = new CommandResult<TEntity>();
            try
            {
                entity.deleted = true;
                _context.Set<TEntity>().Add(entity);
                _context.SetModifedState(entity);
                SaveChanges();
                result.success = true;
                result.messages.Add(Constants.DeleteSuccessMessage);
                result.id = (int)entity.GetType().GetProperty("id").GetValue(entity);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.messages.Add(ex.Message);
            }

            return result;
        }

        public CommandResult<TEntity> Save(TEntity entity)
        {
            CommandResult<TEntity> result = new CommandResult<TEntity>();
            try
            {

                entity.deleted = false;
                if (entity.id > 0)
                {
                    entity.modified_date = DateTime.Now;
                    _context.Set<TEntity>().Add(entity);
                    _context.SetModifedState(entity);
                }
                else
                {
                    entity.created_date = DateTime.Now;
                    entity.modified_date = DateTime.Now;
                    _context.Set<TEntity>().Add(entity);
                    _context.SetAddedState(entity);
                }
                SaveChanges();
                result.success = true;
                result.entity = entity;
                result.messages.Add(Constants.SaveSucessMessage);
                result.id = (int)entity.GetType().GetProperty("id").GetValue(entity);
            }
            catch (Exception ex)
            {
                result.success = false;
                result.messages.Add(ex.Message);
            }
            return result;
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        protected IQueryable<TView> RawSQL<TView>(string query, object[] parameters) where TView : BaseEntity
        {
            if (parameters == null)
            {
                return _context.Set<TView>().FromSqlRaw<TView>(query).AsQueryable();
            }
            else
            {
                return _context.Set<TView>().FromSqlRaw<TView>(query, parameters).AsQueryable();
            }
        }
        protected virtual IQueryable<TEntity> CustomQuery()
        {
            //db.users.where(s=>s.name=="su" && s.id==1);
            return _context.Set<TEntity>().AsQueryable();
        }

        public CommandResult<List<TEntity>> SaveList(List<TEntity> entityList)
        {
            CommandResult<List<TEntity>> result = new CommandResult<List<TEntity>>();
            result.entity = new List<TEntity>();

            try
            {
                foreach (var entity in entityList)
                {
                    entity.deleted = false;

                    if ((int)entity.GetType().GetProperty("id").GetValue(entity) > 0)
                    {
                        entity.modified_date = DateTime.Now;
                        _context.Set<TEntity>().Add(entity);
                        _context.SetModifedState(entity);
                    }
                    else
                    {
                        entity.created_date = DateTime.Now;
                        entity.modified_date = DateTime.Now;
                        _context.Set<TEntity>().Add(entity);
                        _context.SetAddedState(entity);
                    }

                    result.entity.Add(entity);
                }

                SaveChanges();
                result.success = true;
                result.messages.Add(Constants.SaveSucessMessage);
                result.id = result.entity.Count > 0 
                    ? (int)result.entity[0].GetType().GetProperty("id").GetValue(result.entity[0])
                    : 0;
            }
            catch (Exception ex)
            {
                result.success = false;
                result.messages.Add(ex.Message);
            }

            return result;
        }

       
    }
}
