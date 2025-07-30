using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specification;


namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
      //  private readonly IGenericRepositort<Product> _getProduct;
       // private readonly IGenericRepositort<DeliveryMethod> _deliveryMethodRepo;
       // private readonly DbContext _context; // Assuming you have a DataContext for EF Core
      //  private readonly IGenericRepositort<Order> _orderRepo; // Assuming you have an Order repository
        private readonly IUnitOfWork _unitOfWork; // Assuming you have a UnitOfWork for managing transactions
        public OrderService(IBasketRepository basketRepo,IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
           // _getProduct = getProduct;
           // _deliveryMethodRepo = deliveryMethodRepo;
           // _orderRepo = orederRepo;
            _unitOfWork = unitOfWork;
            //  _context = context;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketID, int deliveryMethodId, Address ShippingAddress)
        {
            // Get Basket From BasketRepo 
            var basket= await _basketRepo.GetBasketAsync(basketID);
            //Get Selected Items at Baket From ProductRepo
            var orderItems = new List<OrderItem>();
            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    //use UnitOfWork to get Product by Id
                   // var product =await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                   var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    //var product = await _getProduct.GetByIdAsync(item.Id);
                    if (product == null) throw new Exception($"Product with id {item.Id} not found");
                    var productItemOrdered = new ProductItemOrdered(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }
            //Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);
            //Get DeliveryMethod From DeliveryMethodRepo
            //use UnitOfWork to get DeliveryMethod by Id

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);
            //Create Order Object
            var order = new Order(buyerEmail, ShippingAddress, deliveryMethod, orderItems, subTotal);
            // Add Order locally
            //use UnitOfWork to Add Order
            await _unitOfWork.Repository<Order>().AddAsync(order);
            // Here you would typically add the order to your DbContext and save changes
            // Save Order to DB
            var result = await _unitOfWork.CompleteAsync();
            return order;
         
             
        }

        public async Task<Order> GetOrderByIdForSpecificUserAsync(int id, string buyerEmail)
        {
            //Use specification to get order by id and buyerEmail
            var spec = new OrderWithItemsAndDeliverMethodsSpecification(id, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
             return order;
           // throw new NotImplementedException();
        }
        public async Task<IReadOnlyList<Order>> GetOrdersForSpecificUserAsync(string buyerEmail)
        {
            //Use specification to get all orders for specific user
            var spec = new OrderWithItemsAndDeliverMethodsSpecification(buyerEmail);
            IReadOnlyList<Order>orders=await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders.Where(o => o.BuyerEmail == buyerEmail).ToList();
            //throw new NotImplementedException();
        }
    }
}
