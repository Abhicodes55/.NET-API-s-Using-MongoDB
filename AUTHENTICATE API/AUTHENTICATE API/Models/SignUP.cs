using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace AUTHENTICATE_API.Models
{
    public class SignUP
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID = null;      //Properties declared i.e, Columns of table/collection Signup
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
