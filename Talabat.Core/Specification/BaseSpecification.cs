using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set ; }
        public List<Expression<Func<T, object>>> Includes { get; set; }= new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set ; }
        public Expression<Func<T, object>> OrderByDescending { get ; set ; }
        public int Take { get; set  ; }
        public int Skip { get ; set ; }
        public bool IsPagineantionEnable { get; set ; }

        //Get All
        public BaseSpecification()
        {
            //Includes = new List<Expression<Func<T, object>>>(); 
        }
        //Get By Id
        public BaseSpecification(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
          //  Includes = new List<Expression<Func<T, object>>>();
        }
        public void SetOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        public void SetOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }
        // Set Pagineation
        public void ApplyPagineation(int skip, int take)
        {
            IsPagineantionEnable = true;
            Skip = skip;
            Take = take;   
        }
    }


}
