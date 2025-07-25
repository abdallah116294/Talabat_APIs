using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using NETCore.MailKit.Core;
using System.Net;
using System.Security.Claims;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Servcies;
using Talabat.Service;
using Talabat_APIs.DTO;
using Talabat_APIs.Errors;
using Talabat_APIs.Extensions;

namespace Talabat_APIs.Controllers
{

    public class AccountsController : APIBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService; // Assuming you have a token service for generating JWT tokens
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        public AccountsController(UserManager<AppUser> userManager, SignInManager<AppUser>signInManager,ITokenService tokenService,IEmailService emailService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService; // Initialize the token service
            _emailService = emailService;
            _mapper = mapper;
        }
        //Register
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO register)
        {
            //Check if the email already exists in the database
            var existingUser = await _userManager.FindByEmailAsync(register.Email);
            if (existingUser != null)
            { 
                // If the email already exists, return a BadRequest response with an error message
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Email is already registered"));
            }
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
                //Check if the error due to duplicate email
                var duplicateEmailError = result.Errors.FirstOrDefault(e => e.Code == "Email");
                if (duplicateEmailError != null)
                {
                    return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Email already exists."));
                }
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
        [HttpPost("ForgetPassword")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null)
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "User not found."));
            ///!using OTP for security, you can implement it later
            var otpCode=new Random().Next(100000, 999999).ToString(); // Generate a random OTP code
            //Store the OTP code in the user's record or a secure location
            var result = await _userManager.SetAuthenticationTokenAsync(user, "Talabat", "ResetPasswordOTP", otpCode);
            if (!result.Succeeded)
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Error while setting OTP code."));
            //  var token =await _userManager.GeneratePasswordResetTokenAsync(user);
            // Here you would typically send the token to the user's email address
            //  var encodedToken = WebUtility.UrlEncode(token);
            // Create the reset link with the encoded token
            // var resetLink = $"{Request.Scheme}://{Request.Host}/api/Accounts/ResetPassword?email={WebUtility.UrlEncode(user.Email)}&token={encodedToken}";
            // TODO: Send the reset link via email using your email service
            // Console.WriteLine($"Reset password link: {resetLink}");
            //            var body = $@"
            //    <p>Hello,</p>
            //    <p>Click the link below to reset your password:</p>
            //    <p><a href='{resetLink}'>Reset Password</a></p>
            //    <p>This link will expire shortly.</p>
            //";
            var message = $"Your OTP code is: {otpCode}"; // Message to send via email
            await _emailService.SendEmailAsync(user.Email,"Reset Password Code",message);
            return Ok("OTP send to email ");
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordDTO resetPasswordDTO)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDTO.Email);
            if (user == null)
                return NotFound(new ErrorsApiResponse(StatusCodes.Status404NotFound, "User not found."));
            //Get Stored OTP code from the user's record or secure location
            var storedOtp= await _userManager.GetAuthenticationTokenAsync(user, "Talabat", "ResetPasswordOTP");
            if (storedOtp == null || storedOtp != resetPasswordDTO.Token)
                return BadRequest("Invalid or expired OTP");
            var restToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, restToken, resetPasswordDTO.NewPassword);
            if (!result.Succeeded)
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest,"Error while reset your Password"));
            // Optionally, you can clear the OTP after successful password reset
            await _userManager.RemoveAuthenticationTokenAsync(user, "Talabat", "ResetPasswordOTP");
            return Ok("Password has been reset successfully.");
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
        public async   Task<ActionResult<AddressDTO>> GetCurrentUserAddress() 
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(Email);
            var user = await _userManager.FindUserWithAddressAsync(User);
            if (user == null)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "User not found."));
            }
            var MappedAddress = _mapper.Map<Address, AddressDTO>(user.Address);
            //Agger Loading , Lazy Loading 
            return Ok(new APIResponse<AddressDTO>()
            {
                Data = MappedAddress,
                Status = "Success"
            });
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("UpdateUserAddress")]
        public async Task<ActionResult<AddressDTO>> UpdateUserAddress([FromBody]AddressDTO UpdateAddress) 
        {
             var user = await _userManager.FindUserWithAddressAsync(User);
            if(user == null)
            {
                return Unauthorized(new ErrorsApiResponse(StatusCodes.Status401Unauthorized, "User not found."));
            }
            var MappedAddress = _mapper.Map<AddressDTO, Address>(UpdateAddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var result= await  _userManager.UpdateAsync(user);
            if(!result.Succeeded)
            {
                return BadRequest(new ErrorsApiResponse(StatusCodes.Status400BadRequest, "Error while updating address."));
            }
            return Ok(new APIResponse<AddressDTO>()
            {
                Data = UpdateAddress,
                Status = "Success"
            });
        }
        [HttpGet("emailExists")]
        public async Task<ActionResult<bool>>CheckEmailExists(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
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
