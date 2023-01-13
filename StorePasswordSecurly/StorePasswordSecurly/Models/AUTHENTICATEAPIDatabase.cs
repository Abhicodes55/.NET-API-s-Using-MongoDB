using static StorePasswordSecurly.Models.AUTHENTICATEAPIDatabasesettings;

namespace StorePasswordSecurly.Models
{
    public class AUTHENTICATEAPIDatabasesettings : apiDatabaseSettings//Inherting the interface 
    {
        public string AUTHENTICATEAPICollectionname { get; set; } //Value for these properties is declared in the appsetiing.Development.json file
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public interface apiDatabaseSettings  //Interface that contains Properties for binding data with MongoDb Database
        {
            public string AUTHENTICATEAPICollectionname { get; set; }
            public string ConnectionString { get; set; }
            public string DatabaseName { get; set; }
        }
    }
}
