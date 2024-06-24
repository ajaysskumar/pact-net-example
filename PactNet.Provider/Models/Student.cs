using PactNet.Provider.Shared;

namespace PactNet.Provider.Models
{
    public class Student
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public Standard Standard { get; set; }
    }
}