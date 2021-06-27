using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace POS.Core.Repo
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

        IDbContextTransaction BeginTransaction();
    }
}
