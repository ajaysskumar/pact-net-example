using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PactNet.Provider.Controllers;

namespace PactNet.Provider.UnitTest;

public class StudentControllerTest
{
    [Test]
    public void ShouldReturn200WhenStudentIdExists()
    {
        var mockLogger = new Mock<ILogger<StudentController>>();
        var controller = new StudentController(mockLogger.Object);

        var objectResult = (ObjectResult) controller.GetById(1);
        var student = (Student)objectResult.Value!;
        Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        Assert.That(student, Is.Not.Null);
        Assert.That(student.Id, Is.EqualTo(1));
    }
    
    [Test]
    public void ShouldReturn400WhenStudentIdDoesNotExists()
    {
        var mockLogger = new Mock<ILogger<StudentController>>();
        var controller = new StudentController(mockLogger.Object);

        var objectResult = (ObjectResult) controller.GetById(100);
        Assert.That(objectResult.StatusCode, Is.EqualTo(404));
    }
}