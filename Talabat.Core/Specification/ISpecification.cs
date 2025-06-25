using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public interface ISpecification<T>where T : BaseEntity
    {
        //Signature for Property For Filtering => Where Condition
       public  Expression<Func<T, bool>> Criteria { get; set; }
        //Signature for Property For List of Includes => Include Condition 
        public List<Expression<Func<T,object>>> Includes { get; set; }

    }
}
