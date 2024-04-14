namespace PactNet.ConsumerOne.Models.Events;

public class StudentCreatedEvent
{
    public string StudentId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int StandardId { get; set; }
}