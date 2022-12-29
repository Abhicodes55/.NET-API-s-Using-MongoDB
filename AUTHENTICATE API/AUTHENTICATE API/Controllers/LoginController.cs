using AUTHENTICATE_API.Models;
using AUTHENTICATE_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;

namespace AUTHENTICATE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        public readonly AuthenticateService _authenticateService;//Creating Object for Authenthicate Service and Iconfiguration
        private IConfiguration _config;

        public LoginController(AuthenticateService authenticateService, IConfiguration config)
        {
            _authenticateService = authenticateService;//Initializing Objects using Constructor
            _config = config;
        }

        [HttpPost("Authentication")]
        [AllowAnonymous]//This Api does not Require Authorization i.e  Can be Accessed by any Person
        public IActionResult Authentication(Login login)
        {
            var res = _authenticateService.Authenticate(login.Email, login.Password);//Calls Authenticate Function in Authenticate service and stores response returned from service into res Variable
            if (res == null)
            {
                return Ok("Incorrect Email or Password");
            }
           return Ok(new JWT_Manager(_config).GenerateToken(res.Role,res.Email, res.Name));//IF email and  password is registered and successfully matched 
          //then calls the function to generate JWT Token in JWT_Manager class whic is Returned By the API to Frontend                                                                       

        }
    }
}

