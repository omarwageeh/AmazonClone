using AmazonClone.Dto;
using AmazonClone.Model;
using AmazonClone.Repository.Interface;

namespace AmazonClone.Service
{
    public class UserService
    {
        private readonly IUnitOfWork _uow;
        public UserService(IUnitOfWork uow) 
        {
            _uow = uow;
        }
       public void Register(CustomerDto customer)
        {
            Customer user = new Customer(customer.Address)
            {
                Email = customer.Email,
                Phone = customer.Phone,
            };
            _uow.UserRepository.Add(user);
            _uow.CustomerRepository.Add(user);
            _uow.SaveChanges();
        }
    public async Task<User?> LoginUser(string email, string password)
        {
            var user = await  _uow.UserRepository.Get(u => u.Email == email);
            if (user == null)
            {
                return null;
            }
            return user;
        }
    }
}