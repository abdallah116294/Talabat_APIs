using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepositort<T> where T :BaseEntity 
    {
        #region Without Specification 
        //Get Entity of Type T by Id        
        Task<T> GetByIdAsync(int id);
        //Get All Entities of Type T
        Task<IReadOnlyList<T>> GetAllAsync();
        //Task<T> AddAsync(T entity);
        //Task UpdateAsync(T entity);
        //Task DeleteAsync(T entity);
        //Task<int> CountAsync();
        //Task<bool> ExistsAsync(int id);
        #endregion
        #region With Specification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T>Spec);
        Task<T> GetByIdWithSpecAsync(ISpecification<T>Spec);
        #endregion
    }
}
