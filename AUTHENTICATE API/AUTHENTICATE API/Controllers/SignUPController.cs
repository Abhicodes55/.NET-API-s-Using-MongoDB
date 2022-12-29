using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AUTHENTICATE_API.Models;
using AUTHENTICATE_API.Services;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;

namespace AUTHENTICATE_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignUPController : ControllerBase
    {
        public readonly AuthenticateService _authenticateService;//Creating Object for Authenticate Service 

        public SignUPController(AuthenticateService authenticateService)
        {
            _authenticateService = authenticateService; //Initializing Object in Constructor
        }
        [HttpGet]
        [Authorize(Roles = "Admin,User")]//ROLE BASED AUTHORIZATION (users With certain roles are allowed to access the API)

        public ActionResult<List<SignUP>> GetUser() => _authenticateService.GetUser();//Calls the GetUser function in the service and returns response from service to Frontend

        [HttpGet("{ID}")]
        [Authorize(Roles = "Admin,User")]
        public SignUP Get(string ID) => _authenticateService.GetUserByID(ID);//Calls the GetUserByID function in the service and returns response from service to Frontend

        [HttpPost]
        [AllowAnonymous]//This Api does not Require Authorization i.e  Can be Accessed by any Person
        public IActionResult Post([FromBody] SignUP signUP)
        {
          
             if (signUP.Email.Equals("Admin@gmail.com")&&signUP.Password.Equals("Admin12345"))//If Email and Password Coming From FRONTEND
            {
                signUP.Role = "Admin";//matches with Predifined Admin Email and Pass then "ADMIN" Role is allotted to the User 
            }
            else
            {
                signUP.Role = "User"; //Rest all other Users are Allotted with Role "User"
            }


            var res = _authenticateService.Create(signUP);//Calls the Create Function in Service and stores response from service in res variable

            return Ok(res);//Return Response to Frontend
          
        }

        [HttpDelete("{ID}")]
        [Authorize(Roles = "Admin,User")]//ROLE BASED AUTHORIZATION (users With certain roles are allowed to access the API)
        public ActionResult Delete(String ID)
        {
            var res = _authenticateService.GetUserByID(ID);//Calls the GetUserByID function in the service and stores response from service in res variable
            if (res == null)   //Condition if User Not Found
            {
                return NotFound($" User with ID={ID} Not Found");//Return Equivalent Response to FrontEnd
            }
            //If User is Found
            _authenticateService.Delete(ID);//Calls Delete function in Service which deletes User on the basis of ID
            return Ok($"User Deleted Successfully");//Return Equivalent Response to FrontEnd

        }

        [HttpPut("{ID}")]
        [Authorize(Roles = "Admin,User")]//ROLE BASED AUTHORIZATION (users With certain roles are allowed to access the API)

        public IActionResult Update(String ID, SignUP signup)
        {
            var res = _authenticateService.GetUserByID(ID);//Calls the GetUserByID function in the service and stores response from service in res variable
            if (res == null)//Condition if User Not Found
            {
                return NotFound($"User with ID={ID} Not Found");//Return Equivalent Response to FrontEnd
            }
            //If User is Found
            _authenticateService.Update(ID, signup);//Calls Update function in Service which Updates data of User on the basis of ID
            return Ok($"Record Updated Successfully");//Return Equivalent Response to FrontEnd

        }

    }
}
