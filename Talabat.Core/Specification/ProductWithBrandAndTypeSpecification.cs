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
        public ProductWithBrandAndTypeSpecification(string Sort) : base()
        {
            Includes.Add(p => p.ProductBrand);
            Includes.Add(p => p.ProductType);
            if(!string.IsNullOrEmpty(Sort))
            {
                switch (Sort)
                {
                    case "PriceAsc":
                             SetOrderBy(P=>P.Price);
                        break;
                    case "PriceDesc":
                        SetOrderByDescending(P => P.Price);
                        break;
                    default:
                        SetOrderBy(P => P.Name);
                        break;
                }
            }
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
