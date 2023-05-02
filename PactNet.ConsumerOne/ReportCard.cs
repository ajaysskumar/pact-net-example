using System;

namespace PactNet.ConsumerOne
{
    public class ReportCard
    {
        public int Id { get; set; }
        public StudentDto Student { get; set; }
        public double Score { get; set; }
    }

    public class StudentDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
    }
}