﻿using MessageBroker;
using Microsoft.AspNetCore.Mvc;
using PactNet.Provider.Models;
using PactNet.Provider.Models.Events;
using PactNet.Provider.Repository;

namespace PactNet.Provider.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentRepo _studentRepo;
        private readonly IEventPublisher _eventPublisher;

        public StudentsController(IStudentRepo studentRepo, IEventPublisher eventPublisher)
        {
            _studentRepo = studentRepo;
            _eventPublisher = eventPublisher;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            return Ok(_studentRepo.GetStudentById(id));
        }
        
        [HttpPost]
        public IActionResult Post(Student student)
        {
            var createdStudent = _studentRepo.AddStudent(student);
            _eventPublisher.PublishAsync(new StudentCreatedEvent()
            {
                StudentId = createdStudent.Id,
                Address = createdStudent.Address,
                Gender = createdStudent.Gender,
                StandardId = createdStudent.Standard,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName
            }, "student-created");
            return Ok();
        }
    }
}