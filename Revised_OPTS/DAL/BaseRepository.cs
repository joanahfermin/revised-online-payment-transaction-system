using Inventory_System.DAL;
using Inventory_System.Model;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Revised_OPTS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> getDbSet()
        {
            return ApplicationDBContext.CurrentInstance.Set<T>();
        }

        public T Get(object id)
        {
            return getDbSet().Find(id);
        }

        public List<T> GetAll()
        {
            var query = getDbSet().Take(100).ToList();
            return query;
        }

        public List<T> GetBanks()
        {
            return getDbSet().ToList();
        }

        public void Insert(T entity)
        {
            getDbSet().Add(entity);
        }

        public void Update(T entity)
        {
            ApplicationDBContext.CurrentInstance.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            BaseEntity b =  entity as BaseEntity;
            b.DeletedRecord = 1;
            Update(entity);
        }
    }
}