namespace PactNet.ConsumerOne.Models
{
    public class ReportCard
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public double Score { get; set; }
    }
}