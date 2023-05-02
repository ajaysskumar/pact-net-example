using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PactNet.Provider.Repository;

namespace PactNet.Provider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepo _studentRepo;

        public StudentsController(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_studentRepo.GetStudentById(id));
        }
    }
}