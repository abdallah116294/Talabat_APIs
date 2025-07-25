using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
namespace Talabat.Core.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value ="Pending")]
        Pending, // Order has been created but not yet processed
        [EnumMember(Value = "PaymentRecived")]
        PaymentRecived, // Payment has been received for the order
        [EnumMember(Value = "PaymentFailed")]
        PaymentFailed, //   Payment for the order has failed

    }
}
