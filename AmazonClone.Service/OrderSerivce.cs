using AmazonClone.Model;
using AmazonClone.Repository.Interface;

namespace AmazonClone.Service
{
    public class OrderSerivce
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderSerivce(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;            
        }
        public async Task<Order> AddOrder(Order order)
        {
            return await _unitOfWork.OrderRepository.Add(order);
        }
        public async Task UpdateOrder(Order order)
        {
            _unitOfWork.OrderRepository.Update(order);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<OrderDetails> AddOrderDetails(OrderDetails orderDetails)
        {
            return await _unitOfWork.OrderDetailsRepository.Add(orderDetails);
        }
        public async Task<OrderDetails?> GetOrderDetails(string orderId, string productId)
        {
            return await _unitOfWork.OrderDetailsRepository.Get(od => od.OrderId == Guid.Parse(orderId) && od.ProductId == Guid.Parse(productId));
        }
        public async Task RemoveOrderDetails(string orderId, string productId)
        {
            var orderDetails = await GetOrderDetails(orderId, productId);
            if(orderDetails == null) 
            {
                return;
            }
            _unitOfWork.OrderDetailsRepository.Delete(orderDetails);
            await _unitOfWork.SaveChangesAsync();
        }
        public async void UpdateOrderDetails(OrderDetails orderDetails)
        {
            _unitOfWork.OrderDetailsRepository.Update(orderDetails);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
