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

namespace DMS.DataAccess
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
            PagedResult<TEntity> results = new PagedResult<TEntity>();
            try
            {
                if (option != null)
                {
                    if (option.Page == 0)
                    {
                        option.Page = 1;
                    }
                    int skip = (option.Page - 1) * option.RecordPerPage;
                    if (option.FilterBy != null)
                    {
                        results.total = _context.Set<TEntity>().Where(option.FilterBy).Where(x => x.deleted == false).Count();
                    }
                    else
                    {
                        results.total = _context.Set<TEntity>().Where(x => x.deleted == false).Count();
                    }
                    if (results.total > 0)
                    {
                        var query = _context.Set<TEntity>();

                        if (option.FilterBy != null)
                        {
                            // results.filterBy = option.FilterBy.GetType().GetMembers().ToString();
                            query.Where(option.FilterBy);
                            if (option.SortOrder == SortOrder.ASC)
                            {
                                var q = query.Where(option.FilterBy).Where(x => x.deleted == false).OrderBy(option.SortBy[0]);

                                if (option.SortBy.Count > 1)
                                {
                                    for (int i = 1; i < option.SortBy.Count; i++)
                                    {
                                        q = q.ThenBy(option.SortBy[i]);
                                    }
                                }
                                if (option.legth < 0)
                                {
                                    results.data = q.ToList();
                                }
                                else
                                {
                                    results.data = q.Skip(skip).Take(option.RecordPerPage).ToList();
                                }

                            }
                            else
                            {
                                var q = query.Where(option.FilterBy).Where(x => x.deleted == false).OrderByDescending(option.SortBy[0]);

                                if (option.SortBy.Count > 1)
                                {
                                    for (int i = 1; i < option.SortBy.Count; i++)
                                    {
                                        q = q.ThenByDescending(option.SortBy[i]);
                                    }
                                }
                                if (option.legth < 0)
                                {
                                    results.data = q.Skip(skip).ToList();
                                }
                                else
                                {
                                    results.data = q.Skip(skip).Take(option.RecordPerPage).ToList();
                                }

                            }
                        }
                        else
                        {
                            if (option.SortOrder == SortOrder.DESC)
                            {
                                var q = query.Where(x => x.deleted == false).OrderByDescending(option.SortBy[0]);

                                if (option.SortBy.Count > 1)
                                {
                                    for (int i = 1; i < option.SortBy.Count; i++)
                                    {
                                        q = q.ThenByDescending(option.SortBy[i]);
                                    }
                                }
                                if (option.legth < 0)
                                {
                                    results.data = q.ToList();
                                }
                                else
                                {
                                    results.data = q.Skip(skip).Take(option.RecordPerPage).ToList();
                                }

                            }
                            else
                            {
                                IOrderedEnumerable<TEntity> tmp = query.Where(x => x.deleted == false).OrderBy(option.SortBy[0]);
                                if (option.SortBy.Count > 1)
                                {
                                    for (int i = 1; i < option.SortBy.Count; i++)
                                    {
                                        tmp = tmp.ThenBy(option.SortBy[i]);
                                    }
                                }
                                if (option.legth < 0)
                                {
                                    results.data = tmp.ToList();
                                }
                                else
                                {
                                    results.data = tmp.Skip(skip).Take(option.RecordPerPage).ToList();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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

    }
}
