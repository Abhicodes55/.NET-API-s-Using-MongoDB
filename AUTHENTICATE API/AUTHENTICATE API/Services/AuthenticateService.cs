using MongoDB.Driver;
using AUTHENTICATE_API.Models;
using System.Collections.Generic;
using System.Linq;
using static AUTHENTICATE_API.Models.AUTHENTICATEAPIDatabasesettings;
using System.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AUTHENTICATE_API.Services
{
    public class AuthenticateService  //Service  is used as a middleware between Controllers and the MongoDB database
    {
        private readonly IMongoCollection<SignUP> _users;//Declaring Variable for Signup model using IMongoCollection
        
        public AuthenticateService(apiDatabaseSettings settings)//Declaring Constructor for Service and passing interface as an argument 
        {
            var client = new MongoClient(settings.ConnectionString); //Initialising connection with MongoDB
            var database = client.GetDatabase(settings.DatabaseName);//Intialising database for connection

            _users = database.GetCollection<SignUP>(settings.AUTHENTICATEAPICollectionname);//Initialising Table/Collection in the database 
           

        }

        public List<SignUP> GetUser()//This function returns all the registered users in the databse back to [HTTPGET] call in the signup controller
        {                              
            List<SignUP> Users;
          return  Users = _users.Find(emp => true).ToList();
            
        }
        public SignUP GetUserByID(string ID)//This function returns individual user matched by ID  in the databse back to [HTTPGET{ID}] call in the signup controller
        {
           
           return _users.Find(emp =>emp.ID==ID).FirstOrDefault();
        }

        public string Create( SignUP signUP)
        {    var res = _users.Find(emp => emp.Email.Equals(signUP.Email)).FirstOrDefault();//Query to check if email from Frontend matches exactly with any email registered in the database and result is stored in res variable

            if (res==null)//If res value is null Which means Data is not Found in the Database, hence allowed to resgister
            {
                _users.InsertOne(signUP);//Query to insert data into Collection/Table
                return ("User Registered Successfully");
            }

            return ("Already Registered");//If Email matches with any Record in tha Database,hence not allowed to register
        }
        public void Delete(string id)
        {
            _users.DeleteOne(emp => emp.ID == id);//Query to match ID with exixting records and Delete it from the  Database
            
            
        }
        public SignUP Authenticate(string Email , string Pass) {

            return _users.Find(emp => (emp.Email.Equals(Email)) && (emp.Password.Equals(Pass))).FirstOrDefault();
            //Query to Check if Email and Password Matches with any record in the database the result from this query is returned as a response back to [HTTPOST] call from Login Controller
        }

        public void Update(string ID , SignUP signup)
        {
           _users.ReplaceOne(emp => emp.ID == ID,signup);//Query updates a single record by matching ID in the database and returns Nothing to the function call in the Login Controller 
        }

      
    }
}

