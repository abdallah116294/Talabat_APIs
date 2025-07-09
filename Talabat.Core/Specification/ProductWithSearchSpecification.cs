using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithSearchSpecification:BaseSpecification<Product>
    {
        public ProductWithSearchSpecification(string search) : base(p => string.IsNullOrEmpty(search) || p.Name.ToLower().Contains(search))
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
        public ProductWithSearchSpecification(int id) : base(p => p.Id == id)
        {
            Includes.Add(p => p.ProductBrand);     
            Includes.Add(p => p.ProductType);
        }
    }


}
  
