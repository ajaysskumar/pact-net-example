namespace PactNet.Provider.Models.Events;

public class ReportCardCreatedEvent
{
    public ReportCardCreatedEvent(int id, bool status, int year, int studentId)
    {
        Id = id;
        Status = status;
        Year = year;
        StudentId = studentId;
    }

    public int Id { get; set; }
    public bool Status { get; set; }
    public int Year { get; set; }
    public int StudentId { get; set; }
}