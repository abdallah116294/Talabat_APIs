using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;

namespace Talabat_APIs.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountsController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
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
                Token = "This will be Here A token " // Placeholder for token generation, actual implementation will vary
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
            var result = await _userManager.CheckPasswordAsync(User, login.Password);
            if (!result)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "Invalid email or password."));
            }
            var response = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                //Token= await _userManager.GenerateEmailConfirmationTokenAsync(User)
                Token = "This will be Here A token " // Placeholder for token generation, actual implementation will vary
            };
            return Ok(new APIResponse<UserDTO>()
            {
                Data = response,
                Status = "Success"
            });
        }
    }
}
