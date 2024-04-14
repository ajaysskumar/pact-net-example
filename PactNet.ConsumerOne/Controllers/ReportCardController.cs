using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PactNet.ConsumerOne.HttpClients;
using PactNet.ConsumerOne.Models;
using PactNet.ConsumerOne.Models.Events;

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
        public async Task<IActionResult> GetById(string id)
        {
            var student = await _studentClient.GetStudentById(id);

            var report = new ReportCard()
            {
                Id = Random.Shared.Next(),
                Student = student,
                Score = 2 * 100
            };

            return Ok(report);
        }
        
        [HttpPost]
        public async Task<IActionResult> Post(string id)
        {
            try
            {
                var student = await _studentClient.GetStudentById(id);
                if (student == null)
                {
                    return NotFound();
                }
                var reportCard = new ReportCard()
                {
                    Id = DateTime.Now.Year + Random.Shared.Next(),
                    Student = student,
                    Score = 9.5d
                };
                return Ok(reportCard);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
    }
}