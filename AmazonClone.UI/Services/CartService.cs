using AmazonClone.UI.Models;
using System.Text.Json;

namespace AmazonClone.UI.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Cart> GetCartFromSession()
        {
            await _httpContextAccessor.HttpContext!.Session.LoadAsync();
            var sessionString = _httpContextAccessor.HttpContext.Session.GetString("cart");
            if (sessionString is not null)
            {
                return JsonSerializer.Deserialize<Cart>(sessionString)!;
            }

            return new Cart();
        }
        public void SetCart(Cart cart)
        {
            _httpContextAccessor.HttpContext!.Session.SetString("cart", JsonSerializer.Serialize(cart));
        }
    }
}
