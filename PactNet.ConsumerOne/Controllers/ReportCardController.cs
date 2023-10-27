using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PactNet.ConsumerOne.HttpClients;
using PactNet.ConsumerOne.Models;

namespace PactNet.ConsumerOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportCardController : ControllerBase
    {
        private readonly IStudentClient _studentClient;

        public ReportCardController(IStudentClient studentClient)
        {
            _studentClient = studentClient;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var student = await _studentClient.GetStudentById(id);

            var report = new ReportCard()
            {
                Id = id,
                Student = student,
                Score = id * 100
            };

            return Ok(report);
        }
    }
}