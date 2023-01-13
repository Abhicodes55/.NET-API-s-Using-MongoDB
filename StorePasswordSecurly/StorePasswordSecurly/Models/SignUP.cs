using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace StorePasswordSecurly.Models
{
    public class Signup
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID = null;      //Properties declared i.e, Columns of table/collection Signup
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
    }
}
