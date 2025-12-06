using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository
{
    public interface IGenericRepository<Tentity> where Tentity : class
    {
        List<Tentity> GetAll(Func<IQueryable<Tentity>, IQueryable<Tentity>>? include = null);
        Tentity? GetById(params object[] keys);
        void Add(Tentity entity);
        void Update(Tentity entity);
        void Delete(Tentity entity);
        int Count(Expression<Func<Tentity, bool>>? predicate = null);
    }
}
