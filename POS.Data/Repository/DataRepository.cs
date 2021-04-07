using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Repository
{
    public class DataRepository<T> : IGenericDataRepository<T> where T : class
    {
        private POSDataContext ctx = null;
        private DbSet<T> entity = null;
        public DataRepository(POSDataContext context)
        {
            ctx = context;
            entity = ctx.Set<T>();
        }

        public void Delete(int id)
        {
            T existing = entity.Find(id);
            if(existing != null)
            {
                using (ctx)
                {
                    ctx.Entry<T>(existing).State = EntityState.Modified;
                }
            }
            
        }

        public IQueryable<T> GetAll()
        {
            return entity.AsQueryable<T>();
        }

        public T GetByID(int id)
        {
            return entity.Find(id);
        }

        public void Insert(T obj)
        {
            entity.Add(obj);
        }

        public int Save()
        {
            using ( ctx )
            {
                return ctx.SaveChanges();
            }
        }

        public async Task<int> SaveAsync()
        {
            using ( ctx )
            {
                return await ctx.SaveChangesAsync();
            }
        }

        public void Update(T obj)
        {
            entity.Attach(obj);
            using ( ctx )
            {
                ctx.Entry<T>(obj).State = EntityState.Modified;
            }
        }
    }
}
