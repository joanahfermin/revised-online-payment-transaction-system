using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revised_OPTS.DAL
{
    internal interface IRepository<T> where T : class
    {
        List<T> GetAll();
        T Get(object id);
        List<T> GetBanks();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
