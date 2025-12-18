using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        protected readonly CarNestDBContext dbcontext;

        public GenericRepository(CarNestDBContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }




        public async Task<List<Tentity>> GetAllAsync(Func<IQueryable<Tentity>, IQueryable<Tentity>>? include = null)
        {
            IQueryable<Tentity> query = dbcontext.Set<Tentity>();

            if (include != null)
                query = include(query);

            return await query.AsNoTracking().ToListAsync();
        }



        public async Task<Tentity?> GetByIdAsync(params object[] keys)
        {
            return await dbcontext.Set<Tentity>().FindAsync(keys);
        }

        public void Add(Tentity entity)
        {
            dbcontext.Set<Tentity>().Add(entity);
        }
        public void Update(Tentity entity)
        {
            dbcontext.Set<Tentity>().Update(entity);
        }
        public void Delete(Tentity entity)
        {
            dbcontext.Set<Tentity>().Remove(entity);
        }



        public async Task<int> CountAsync(Expression<Func<Tentity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return await dbcontext.Set<Tentity>().CountAsync();
            }
            return await dbcontext.Set<Tentity>().Where(predicate).CountAsync();
        }



        public async Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate)
        {
            return await dbcontext.Set<Tentity>().AnyAsync(predicate);
        }
    }
}

