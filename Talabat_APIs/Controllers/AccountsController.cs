using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Servcies;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService; // Assuming you have a token service for generating JWT tokens
        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser>signInManager,ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService; // Initialize the token service
        }
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            var User = new AppUser()
            {
                DisplayName = register.DisplayName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                UserName = register.Email.Split('@')[0] // Extract username from email
            };
            // Add User to the database (this is a placeholder, actual implementation will vary)
            var result = await _userManager.CreateAsync(User, register.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest));
            }
            var response = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                //Token= await _userManager.GenerateEmailConfirmationTokenAsync(User)
                Token = await _tokenService.CreateTokenAsync(User,_userManager)    // Placeholder for token generation, actual implementation will vary
            };
            return Ok(new APIResponse<UserDTO>()
            {
                Data = response,
                Status = "Success"
            });
        }
        //Login
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO login)
        {
            var User = await _userManager.FindByEmailAsync(login.Email);
            if (User == null)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "Invalid email or password."));
            }
            //var result = await _userManager.CheckPasswordAsync(User, login.Password);
            //if (!result)
            //{
            //    return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "Invalid email or password."));
            //}
            var signInResult = await _signInManager.CheckPasswordSignInAsync(User, login.Password,false);
            if (!signInResult.Succeeded)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "Invalid email or password."));
            }
            var response = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                //Token= await _userManager.GenerateEmailConfirmationTokenAsync(User)
                Token = await _tokenService.CreateTokenAsync(User, _userManager)// Placeholder for token generation, actual implementation will vary
            };
            return Ok(new APIResponse<UserDTO>()
            {
                Data = response,
                Status = "Success"
            });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            // This method retrieves the currently logged-in user based on the JWT token
            // and returns their details along with a JWT token.
            var email=    User.FindFirstValue(ClaimTypes.Email)??User.FindFirst("email")?.Value;
            var user= await _userManager.FindByEmailAsync(email);
            if (user == null)       
          
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "User not found."));
            }
            var response = new UserDTO()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                //Token= await _userManager.GenerateEmailConfirmationTokenAsync(User)
                Token = await _tokenService.CreateTokenAsync(user, _userManager)// Placeholder for token generation, actual implementation will vary
            };
            return Ok(new APIResponse<UserDTO>()
            {
                Data = response,
                Status = "Success"
            });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("GetUserAddress")]
        public async   Task<ActionResult<Address>> GetCurrentUserAddress() 
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "User not found."));
            }
            //Agger Loading , Lazy Loading 

            var response = new Address()
            {
                FirstName = user.DisplayName,
                LastName = user.UserName,
                City = user.Address.City,
                Street = user.Address.Street,
                Country = user.Address.Country,
            };
            return Ok(new APIResponse<Address>()
            {
                Data = response,
                Status = "Success"
            });
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers() 
        {
            var Users = _userManager.Users.ToList();
            if (Users.IsNullOrEmpty())
            {
                return NotFound("there is no User Found ");
            }
            var result = Users.Select(u => new UserDTO
            {
                DisplayName = u.DisplayName,
                Email = u.Email,
            });

            return Ok(new APIResponse<IEnumerable<UserDTO>>()
            {
                Data = result,
                Status = "success"
            });
        }
    }
}
