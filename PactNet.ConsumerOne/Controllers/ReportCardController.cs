using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PactNet.ConsumerOne.HttpClients;

namespace PactNet.ConsumerOne.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportCardController : ControllerBase
    {

        private readonly ILogger<ReportCardController> _logger;
        private readonly IStudentClient _studentClient;

        public ReportCardController(ILogger<ReportCardController> logger, IStudentClient studentClient)
        {
            _logger = logger;
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