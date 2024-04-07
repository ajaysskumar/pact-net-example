using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MessageBroker;
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
        private readonly IEventPublisher _eventPublisher;

        public ReportCardController(IStudentClient studentClient, IEventPublisher eventPublisher)
        {
            _studentClient = studentClient;
            _eventPublisher = eventPublisher;
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
        
        [HttpPost]
        public async Task<IActionResult> Post(int id)
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
                    Id = DateTime.Now.Year + id,
                    Student = student,
                    Score = 9.5d
                };
                await _eventPublisher.PublishAsync(new ReportCardCreatedEvent(reportCard.Id, reportCard.IsPassed, reportCard.Year, reportCard.Student.Id), "result-created");
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