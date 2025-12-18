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

        Task<List<Tentity>> GetAllAsync(Func<IQueryable<Tentity>, IQueryable<Tentity>>? include = null);

        Task<Tentity?> GetByIdAsync(params object[] keys);
        void Add(Tentity entity);
        void Update(Tentity entity);
        void Delete(Tentity entity);

        Task<int> CountAsync(Expression<Func<Tentity, bool>>? predicate = null);

        Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate);
    }
}
