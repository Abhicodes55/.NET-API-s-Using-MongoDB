using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorePasswordSecurly.Models;
using StorePasswordSecurly.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StorePasswordSecurly.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUPController : ControllerBase
    {
        // GET: api/<LoginController>
        public readonly AuthenticateService _authenticateService;//Creating Object for Authenticate Service 

        public SignUPController(AuthenticateService authenticateService)
        {
            _authenticateService = authenticateService; //Initializing Object in Constructor
        }
        [HttpGet]
        public ActionResult<List<Signup>> GetUser() => _authenticateService.GetUser();//Calls the GetUser function in the service and returns response from service to Frontend
        
        [HttpPost]

        [HttpPost]
        [AllowAnonymous]//This Api does not Require Authorization i.e  Can be Accessed by any Person
        public IActionResult Post([FromBody] Signup signUP)
        {

            var pass = _authenticateService.GenerateSecurePass(signUP.Password);
            signUP.Password = pass;
            var res = _authenticateService.Create(signUP);//Calls the Create Function in Service and stores response from service in res variable

            return Ok(res);//Return Response to Frontend

        }

    }
}
