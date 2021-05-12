using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Model;


namespace POS.Data.Repository
{
    public class DataRepository<T> : IGenericDataRepository<T> where T : EntityBase
    {
        private POSDataContext ctx = null;
        private DbSet<T> entity = null;
        public DataRepository(POSDataContext context)
        {
            ctx = context;
            entity = ctx.Set<T>();
        }

        public void Delete(Int64 id)
        {
            T existing = entity.Find(id);
            if (existing != null)
            {
                ctx.Entry<T>(existing).State = EntityState.Deleted;
            }

        }

        public IQueryable<T> GetAll()
        {
            return entity.AsQueryable<T>().AsNoTracking();
        }

        public T GetByID(int id)
        {
            return entity.Single(x=> x.Id == id);
        }

        public bool HasChanges()
        {
            return ctx.ChangeTracker.HasChanges();
        }

        public void Insert(T obj)
        {
            entity.Add(obj);
        }

        public int Save()
        {
            using (ctx)
            {
                return ctx.SaveChanges();
            }
        }

        public async Task<int> SaveAsync()
        {
            using (ctx)
            {
                return await ctx.SaveChangesAsync();
            }
        }

        public void Update(T obj)
        {
            entity.Attach(obj);
            ctx.Entry<T>(obj).State = EntityState.Modified;
        }
    }
}
