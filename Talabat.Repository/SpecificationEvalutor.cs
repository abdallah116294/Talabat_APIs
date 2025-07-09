using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specification;

namespace Talabat.Repository
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var Query = inputQuery;
            if(specification.Criteria != null)
            {
                Query = Query.Where(specification.Criteria);
            }
            //Check if OrderBy is not null and then apply it
            if(specification.OrderBy != null)
            {
                Query = Query.OrderBy(specification.OrderBy);
            }
            //Check if OrderByDescending is not null and then apply it
            if (specification.OrderByDescending != null)
            {
                Query = Query.OrderByDescending(specification.OrderByDescending);
            }
            if (specification.IsPagineantionEnable)
            {
                Query = Query.Skip(specification.Skip).Take(specification.Take);
            }
            //use Aggregate Function to Include Multiple Includes
            Query
               = specification.Includes.Aggregate(Query, (current, include) => current.Include(include));
            //foreach (var include in specification.Includes)
            //{
            //    Query = Query.Include(include);
            //}

            return Query;
        }
    }
}
