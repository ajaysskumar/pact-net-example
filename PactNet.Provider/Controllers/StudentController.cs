using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace PactNet.Provider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private static readonly Student[] Students = new[]
        {
           new Student
           {
               Id = 1,
               FirstName = "Raju",
               LastName = "Rastogi",
               Address = "Delhi",
               Gender = "Male"
           },
           new Student
           {
               Id = 2,
               FirstName = "Farhan",
               LastName = "Akhtar",
               Address = "Lucknow",
               Gender = "Male"
           },
           new Student
           {
               Id = 3,
               FirstName = "Rancho",
               LastName = "Chanchad",
               Address = "Delhi",
               Gender = "Male"
           },
           new Student
           {
               Id = 4,
               FirstName = "Priya",
               LastName = "Chanchad",
               Address = "Delhi",
               Gender = "Female"
           }
        };

        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<Student> Get()
        {
            var rng = new Random();
            return Students;
        }
        
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var student = Students.FirstOrDefault(x => x.Id == id);
            
            return student == null ? NotFound("Student not found") : Ok(student);
        }
    }
}