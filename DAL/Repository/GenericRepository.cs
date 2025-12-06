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


        public List<Tentity>? GetAll(Func<IQueryable<Tentity>, IQueryable<Tentity>>? include = null)
        {
            IQueryable<Tentity> query = dbcontext.Set<Tentity>();

            if (include != null)
                query = include(query);

            return query.AsNoTracking().ToList();
        }

        public Tentity? GetById(params object[] keys)
        {
            return dbcontext.Set<Tentity>().Find(keys);
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

        public int Count(Expression<Func<Tentity, bool>>? predicate = null)
        {
            if (predicate == null)
            {
                return dbcontext.Set<Tentity>().Count();
            }
            return dbcontext.Set<Tentity>().Where(predicate).Count();
        }

    }
}

