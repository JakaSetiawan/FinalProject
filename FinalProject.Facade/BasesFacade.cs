using CodeID.Helper;
using FinalProject.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace FinalProject.Facade
{
    public class BasesFacade<TEntity> : IDisposable where TEntity : class
    {
        public LibSysContext Context = null;

        public BasesFacade()
        {
            Context = new LibSysContext();
        }

        public virtual async Task<IList<TEntity>> GetAll()
        {
            try
            {
                return await Context.Set<TEntity>().ToListAsync<TEntity>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual TEntity GetByID(object id)
        {
            try
            {
                return Context.Set<TEntity>().Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<PageResult> GetPage(PageRequest option)
        {
            try
            {
                var result = await Task.Run(() => {
                    var where = " 1=1 ";
                    var param = new object[option.Criteria.Count];
                    int i = 0;
                    option.Criteria.ForEach(c => {
                        where += string.Format("AND {0}.ToString().Contains(@{1}) ", c.criteria, i);
                        param[i] = c.value;
                        i++;
                    });
                    var query = Context.Set<TEntity>().Where(where, param);
                    var count = query.Count();
                    var rows = query.OrderBy(option.Order)
                        .Skip((option.Page - 1) * option.PageSize)
                        .Take(option.PageSize)
                        .ToList();
                    var pageCount = (int)Math.Ceiling(count * 1.0 / option.PageSize);
                    var pageResult = new PageResult { Rows = rows, RowCount = count, PageCount = pageCount };
                    return pageResult;
                });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> Insert(TEntity entity)
        {
            try
            {
                Context.Set<TEntity>().Add(entity);
                int affctd = await Context.SaveChangesAsync();
                return affctd > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            try
            {
                Context.Entry<TEntity>(entity).State = EntityState.Modified;
                int affctd = await Context.SaveChangesAsync();
                return affctd > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<bool> Delete(object id)
        {
            try
            {
                TEntity entity = GetByID(id);
                if (entity == null)
                {
                    throw new Exception("Data not found!");
                }
                Context.Set<TEntity>().Remove(entity);
                int affctd = await Context.SaveChangesAsync();
                return affctd > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Dispose()
        {

        }
    }
}
