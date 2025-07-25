﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class ProductWithFiltrationForCountAsync:BaseSpecification<Product>
    {
        public ProductWithFiltrationForCountAsync(ProductSpecParams Params ):base(
            P =>
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId)
            && (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId)
            )
        {
            
        }
  
    }
}
