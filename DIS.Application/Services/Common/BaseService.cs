using DIS.Application.Service.Common;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Common;
using DIS.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Application.Services.Common
{
    public abstract class BaseService<TEntity, TEntityID, TRepo> where TEntity : BaseEntity
         where TRepo : IReadWriteRepositoryBase<TEntity>
    {
        protected IUnitOfWork uom;
        protected TRepo repo;
        protected Logger logger;
        protected TEntity user;
        public BaseService(TRepo repo, IUnitOfWork _uom, Logger logger)
        {
            this.logger = logger;
            this.repo = repo;
            this.uom = _uom;
        }

        public virtual CommandResultModel SaveorUpdate(TEntity entity)
        {
            CommandResultModel result = new CommandResultModel();
            try
            {
                if (Duplicate(entity))
                {
                    result.success = false;
                    result.messages.Add("Duplicate!");
                }
                else
                {
                    if (BeforeSaveorUpdate(ref result, entity))
                    {
                        repo.Save(entity);
                        AfterSaveorUpdate(entity);
                        uom.Commit();
                        //result.id = (int)entity.GetType().GetProperty("ID").GetValue(entity);
                        result.success = true;
                        result.messages.Add("Successfully saved!");
                    }
                }
            }

            catch (Exception ex)
            {
                
                result.success = false;
                result.messages.Add(ex.Message);
            }

            return result;
        }
        //public virtual CommandResultModel SaveorUpdateList(List<TEntity> entitylist)
        //{
        //    CommandResultModel result = new CommandResultModel();
        //    try
        //    {

        //        repo.Save(entitylist);
        //        AfterSaveorUpdateList(entitylist);
        //        uom.Commit();
        //        result.success = true;
        //        result.messages.Add("Successfully saved!");


        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Log(ex);
        //        result.success = false;
        //        result.messages.Add("Error");
        //    }

        //    return result;
        //}
        public virtual bool Duplicate(TEntity entity)
        {
            bool duplicated = false;

            return duplicated;
        }

        public virtual bool BeforeSaveorUpdate(ref CommandResultModel result, TEntity entity)
        {
            return true;
        }

        public virtual void AfterSaveorUpdate(TEntity entity)
        {
        }
        public virtual void AfterSaveorUpdateList(List<TEntity> entity)
        {
        }

        public virtual CommandResultModel Delete(TEntity entity)
        {
            CommandResultModel result = new CommandResultModel();
            try
            {
                if (entity != null)
                {
                    repo.Remove(entity);
                    uom.Commit();
                    result.success = true;
                    result.messages.Add("Successfully deleted!");
                }

            }
            catch (Exception ex)
            {
                //logger.Log(ex);
                result.success = false;
                result.messages.Add("Error");

            }

            return result;
        }


    }
}
