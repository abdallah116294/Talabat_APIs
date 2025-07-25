using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Status)
                .HasConversion(OStatus=>OStatus.ToString(),OStatus=>(OrderStatus)Enum.Parse(typeof(OrderStatus),OStatus));
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,2)"); // Set precision and scale for SubTotal
            builder.OwnsOne(o => o.ShippingAddress, X => X.WithOwner());
            builder.HasOne(o=>o.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction); // Ensure that if a delivery method is deleted, related orders are also deleted
        }
    }
}
