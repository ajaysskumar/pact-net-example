using PactNet.Provider.Shared;

namespace PactNet.Provider.Models.Events
{
    public class StudentCreatedEvent
    {
        public string StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; } // Changed from LastName to LName
        public string Address { get; set; }
        public string Gender { get; set; }
        public Standard StandardId { get; set; }
    }
}