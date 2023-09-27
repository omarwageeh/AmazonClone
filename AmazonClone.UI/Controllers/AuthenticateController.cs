using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AmazonClone.Model;

namespace AmazonClone.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<Customer> userManager;
        //private readonly UserManager<Customer> customerManager;
        private readonly SignInManager<Customer> signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserStore<Customer> _userStore;
        private readonly IUserEmailStore<Customer> _userEmailStore;
        //private string FullName;

        public AuthenticateController(UserManager<Customer> userManager, IConfiguration configuration, SignInManager<Customer> signInManager, IUserStore<Customer> userStore)
        {
            this.userManager = userManager;
            //this.customerManager = customerManager;
            this.signInManager = signInManager;
            _configuration = configuration;
            _userStore = userStore;
            _userEmailStore = GetEmailStore();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var isLogged = await signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);
            if (isLogged.Succeeded)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            Customer user = new("Address");
            user.FullName = model.FullName;
            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await _userEmailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again. " + result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description) });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        //public async Task<IActionResult> RegisterCustomer([FromBody] RegisterModel model)
        //{
        //    var userExists = await customerManager.FindByEmailAsync(model.Email);
        //    if (userExists != null)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

        //    Customer customer = new("ay haga");
        //    customer.FullName = model.FullName;
        //    await _userStore.SetUserNameAsync(customer, model.Email, CancellationToken.None);
        //    await _userEmailStore.SetEmailAsync(customer, model.Email, CancellationToken.None);
        //    var result = await customerManager.CreateAsync(customer, model.Password);
        //    if (!result.Succeeded)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again. " + result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description) });

        //    return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        //}

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.FullName);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            Customer user = new Customer("Address")
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                FullName = model.FullName
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
        private IUserEmailStore<Customer> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("loo");
            }
            return (IUserEmailStore<Customer>)_userStore;
        }
    }
}
