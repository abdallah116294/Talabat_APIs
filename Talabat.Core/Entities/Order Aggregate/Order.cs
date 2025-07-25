using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order:BaseEntity
    {
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> itmes, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Itmes = itmes;
            SubTotal = subTotal;
        }

        public Order()
        {
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now; //the time that order created 

        public OrderStatus Status { get; set; } = OrderStatus.Pending;//Defualt value is Pending 
        //Aggregation RelationShip =>Has a ShippingAddress 
        public Address ShippingAddress { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        //Navigation Property for many 
        public ICollection<OrderItem> Itmes { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }// Price of product * Quantity
        //[NotMapped]
        //public decimal Total { get => SubTotal + DeliveryMethod.Cost; } // SubTotal+DeliveryMethod Cost 
        //make it as methods
         public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; } = string.Empty;
       


    }
}
