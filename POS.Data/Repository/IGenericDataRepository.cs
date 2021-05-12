using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public interface IGenericDataRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetByID(int id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(Int64 id);
        int Save();
        Task<int> SaveAsync();

        bool HasChanges();
    }
}
