using eCommerceOrders.FinishPurchase.Models;
using eCommerceOrders.FinishPurchase.Persistence;
using eCommerceOrders.FinishPurchase.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eCommerceOrders.FinishPurchase.Services
{
    public class OrderService : IOrderService
    {
        private readonly ECommerceContext _context;
        private readonly IBusService _bus;

        public OrderService(ECommerceContext context, IBusService bus)
        {
            _context = context;
            _bus = bus;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _context.Order.Include(o => o.OrderProducts)
                                        .ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _context.Order.Include(o => o.OrderProducts)
                                        .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.TotalValue = CalculateTotalValue(order);

            _context.Order.Add(order);
            await _context.SaveChangesAsync();
            
            if (VerifyOrderIsFinished(order))
            {
                await PublishPurchaseFinishedMessage(order);
            }

            return order;
        }

        public async Task<Order> UpdateOrderAsync(Order order)
        {
            order.TotalValue = CalculateTotalValue(order);

            _context.Order.Update(order);
            await _context.SaveChangesAsync();

            if (VerifyOrderIsFinished(order))
            {
                await PublishPurchaseFinishedMessage(order);
            }

            return order;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            var order = await _context.Order.FindAsync(id);

            if (order == null)
            {
                return false;
            }

            var orderProducts = GetOrderProductByOrderId(id);

            if(orderProducts != null)
            {
                _context.OrderProduct.RemoveRange(orderProducts);
            }

            _context.Order.Remove(order);
            await _context.SaveChangesAsync();

            return true;
        }

        private decimal CalculateTotalValue(Order order)
        {
            return order.OrderProducts.Sum(op => op.Product.Price * op.Quantity);
        }

        private List<OrderProduct> GetOrderProductByOrderId(int orderId)
        {
            return _context.OrderProduct.Where(x => x.OrderId == orderId).ToList();
        }

        private bool VerifyOrderIsFinished(Order order)
        {
            if(order.IsFinished)
            {
                return true;
            }

            return false;
        }

        private async Task PublishPurchaseFinishedMessage(Order order)
        {
            var @event = order;

            _bus.Publish(@event);
        }
    }
}