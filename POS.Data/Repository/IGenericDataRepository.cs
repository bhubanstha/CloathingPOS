using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public interface IGenericDataRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        T GetByID(Int64 id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(Int64 id);
        int Save();
        Task<int> SaveAsync();

        bool HasChanges();

        DbContextTransaction BeginTransaction();
    }
}
