using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specification
{
    public class OrderWithItemsAndDeliverMethodsSpecification:BaseSpecification<Order>
    {
        public OrderWithItemsAndDeliverMethodsSpecification(string buyerEmail) : base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.Itmes);
            Includes.Add(o => o.DeliveryMethod);
            SetOrderByDescending(o => o.OrderDate);
        }
        public OrderWithItemsAndDeliverMethodsSpecification(int id, string buyerEmail) : base(o => o.Id == id && o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.Itmes);
            Includes.Add(o => o.DeliveryMethod);
        }
    }
}
