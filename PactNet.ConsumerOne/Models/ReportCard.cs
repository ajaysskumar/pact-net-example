namespace PactNet.ConsumerOne.Models
{
    public class ReportCard
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public double Score { get; set; }
        public int Year { get; set; }
        public bool IsPassed => Score > 5d;
    }
}