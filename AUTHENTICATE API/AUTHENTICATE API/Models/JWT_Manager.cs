using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AUTHENTICATE_API.Models
{
    public class JWT_Manager //This Class is Declared to Generate JWT Token for Secure Login and Providing Security to the Api's(Securing API Endpoints)
    {   //Initialising Properties which will be used for generating the JWT Token
        public string SecretKey { get; set; }  // Properties will get values from appsetting.Development.Json file
        public int TokenDuration { get; set; }
        public object Configuration { get; private set; }
        public IConfiguration Config { get; }

       
        public JWT_Manager( IConfiguration _config)//Constructor For initialising Values
        {
            Config = _config;
            SecretKey = Config.GetSection("Jwt").GetSection("key").Value;//initialising secret key 
            TokenDuration = Int32.Parse(Config.GetSection("Jwt").GetSection("Duration").Value);//Initialising Token Duration
            

           
        }

       //Function to Generate JWT Token and returns a JWT Token back to function Call in Login Controller

        public string GenerateToken(string Role,String Email,string Name)
        {
            var Tokenhanndler = new JwtSecurityTokenHandler();//Initialising TokenHandler
            var TokenK_Key=Encoding.UTF8.GetBytes(SecretKey);//Secret Key Encoded into Bytes
            var Payload = new SecurityTokenDescriptor//Initialising PAYLOAD(data that is sent inside Token )can be seen when decrypted
            {
                Subject = new ClaimsIdentity(
                    new Claim[] { new Claim(ClaimTypes.Role, Role), new Claim(ClaimTypes.Name, Name), new Claim(ClaimTypes.Email, Email) }
                    ),
                Expires= DateTime.Now.AddMinutes(5),//Initialising Expiry time of token
                SigningCredentials =new SigningCredentials(new SymmetricSecurityKey(TokenK_Key),SecurityAlgorithms.HmacSha256)//Initialising Signing Credentials with Security algorithms

            };
            var Token=Tokenhanndler.CreateToken(Payload);//Creates JWT token using Payload
            string FinalToken=Tokenhanndler.WriteToken(Token);//Serailizes JWT into JWE or JWS
            return FinalToken;//Returning Token Back to Function Call

        }
    }
}
