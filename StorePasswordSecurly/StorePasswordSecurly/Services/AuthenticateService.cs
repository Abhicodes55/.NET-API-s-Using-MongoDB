using MongoDB.Driver;
using StorePasswordSecurly.Models;
using System.Security.Cryptography;
using static StorePasswordSecurly.Models.AUTHENTICATEAPIDatabasesettings;

namespace StorePasswordSecurly.Services
{
    public class AuthenticateService
    {
        private readonly IMongoCollection<Signup> _users;//Declaring Variable for Signup model using IMongoCollection

        public AuthenticateService(apiDatabaseSettings settings)//Declaring Constructor for Service and passing interface as an argument 
        {
            var client = new MongoClient(settings.ConnectionString); //Initialising connection with MongoDB
            var database = client.GetDatabase(settings.DatabaseName);//Intialising database for connection

            _users = database.GetCollection<Signup>(settings.AUTHENTICATEAPICollectionname);//Initialising Table/Collection in the database 


        }
        public List<Signup> GetUser()
        {
            List<Signup> Users;
            return Users = _users.Find(emp => true).ToList();

        }
        public string Create(Signup signUP)
        {
            var res = _users.Find(emp => emp.Email.Equals(signUP.Email)).FirstOrDefault();//Query to check if email from Frontend matches exactly with any email registered in the database and result is stored in res variable

            if (res == null)//If res value is null Which means Data is not Found in the Database, hence allowed to resgister
            {
                _users.InsertOne(signUP);//Query to insert data into Collection/Table
                return ("User Registered Successfully");
            }

            return ("Already Registered");//If Email matches with any Record in tha Database,hence not allowed to register
        }

        public string GenerateSecurePass(string Pass)
        {
            byte[] salt;//Declaring Byte Array for Storing SALT
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);//In order to generate the random salt,
                                                                         //we’ll be using RNGCryptoServiceProvider() whose sole job is to generate random numbers. We’ll be storing these numbers in a byte array. 
            var hashpass = new Rfc2898DeriveBytes(Pass, salt, 10000);//The Rfc2989DeriveBytes class is going to be responsible for the hashing. 10000 represents the number of iterations the algorithm is going to perform
                                                                     //(It will keep hashing the previous hash this number of times. This is what makes it slower by design).
            byte[] hash = hashpass.GetBytes(20);//
                                                //Using, yet again, a byte array, we’ll add the salt in front of the hashed password (this is called prepending).
                                                //The size of the array is going to be 36 bytes, because both the hash and salt are fixed length: the hash is 20 bytes, and the salt 16.
            byte[] SecurePass = new byte[36];
            Array.Copy(salt, 0, SecurePass, 0, 16);//Storing SALT and hashed Password in final byte[] array
            Array.Copy(hash, 0, SecurePass, 16, 20);//Storing SALT and hashed Password in final byte[] array
            String SecuredPasswordHash = Convert.ToBase64String(SecurePass); //After adding the hashed password and its salt to the Byte array, we should convert it to a string
            return SecuredPasswordHash;//Returning Encrypted Password as string
        }

    }
}
