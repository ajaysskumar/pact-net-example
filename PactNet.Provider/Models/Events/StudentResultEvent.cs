namespace PactNet.Provider.Models.Events;

public class ReportCardCreatedEvent(int id, bool status, int year, int studentId)
{
    public int Id { get; set; } = id;
    public bool Status { get; set; } = status;
    public int Year { get; set; } = year;
    public int StudentId { get; set; } = studentId;
}