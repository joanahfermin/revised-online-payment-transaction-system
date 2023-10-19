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
        protected ApplicationDBContext dBContext = ApplicationDBContext.Instance;

        protected DbSet<T> dbSet;

        public BaseRepository()
        {
            dbSet = dBContext.Set<T>();
        }

        public T Get(object id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll()
        {
            var query = dbSet.Take(100).ToList();
            return query;
        }

        public List<T> GetBanks()
        {
            return dbSet.ToList();
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
            dBContext.SaveChanges();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
