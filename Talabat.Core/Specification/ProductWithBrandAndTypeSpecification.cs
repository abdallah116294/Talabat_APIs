using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification() : base()
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
        public ProductWithBrandAndTypeSpecification(int id) : base(p=>p.Id==id)
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
        }
        //public ProductWithBrandAndTypeSpecification(int id) : base(p => p.Id == id)
        //{
        //    AddInclude(p => p.ProductBrand);
        //    AddInclude(p => p.ProductType);
        //}
    }
}
