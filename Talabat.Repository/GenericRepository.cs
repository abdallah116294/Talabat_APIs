using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specification;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepositort<T> where T : BaseEntity
    {
        private readonly StoreContext _storeContext;
        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        #region Without Specification
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            if (typeof(T) == typeof(Product))
            {
                return (IReadOnlyList<T>)await _storeContext.Products.Include(p => p.ProductBrand).Include(p => p.ProductType).ToListAsync();
            }
            return await _storeContext.Set<T>().ToListAsync();
            //Applay the specification Design Pattern=> To make Query Dynamic 
            ///1. Entery Point (Start of Query) => _storeContext.Products
            ///2.Where Condition => Where(P=>P.ID==Id)
            ///3.List of includes => Include(P=>P.ProductType).Include(P=>P.ProductBrand)
            /// 
        }



        public async Task<T> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                var product = await _storeContext.Products
               .Include(p => p.ProductType)
                .Include(p => p.ProductBrand)
             .FirstOrDefaultAsync(p => p.Id == id);
                return product as T;
            }
            return await _storeContext.Set<T>().FindAsync(id);
        } 
        #endregion

        #region With Specification
        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
           return await ApplaySpecification(Spec).ToListAsync();
        }
        public async Task<T> GetByIdWithSpecAsync(ISpecification<T> Spec)
        {
           return await ApplaySpecification(Spec).FirstOrDefaultAsync();
        } 
        private IQueryable<T>ApplaySpecification(ISpecification<T> spec)
        {
            return SpecificationEvalutor<T>.GetQuery(_storeContext.Set<T>(), spec);
        }
        #endregion


        //return await  _storeContext.Set<T>().Where(X=>X.Id==id).FirstOrDefaultAsync();
    }
}
