using Inventory_System.DAL;
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
        //protected ApplicationDBContext dBContext = ApplicationDBContext.Instance;
        //protected SecondApplicationDBContext secondDbContext = SecondApplicationDBContext.Instance;

        protected DbContext dBContext; 
        protected DbSet<T> dbSet;

        public BaseRepository(DbContext dBContext)
        {
            dbSet = dBContext.Set<T>();
            this.dBContext = dBContext;
        }

        public T Get(object id)
        {
            return dbSet.Find(id);
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