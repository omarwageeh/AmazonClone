using AmazonClone.Data.Context;
using AmazonClone.Model;
using AmazonClone.Repository.Interface;

namespace AmazonClone.Repository.Repository
{
    public class OrderDetailsRepository : GenericRepository<OrderDetails>, IOrderDetailsRepository
    {
        public OrderDetailsRepository(DataContext context) : base(context)
        {
        }
    }
}
