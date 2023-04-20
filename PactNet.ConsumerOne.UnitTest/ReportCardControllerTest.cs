using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PactNet.ConsumerOne.Controllers;
using PactNet.ConsumerOne.HttpClients;

namespace PactNet.ConsumerOne.UnitTest;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task ShouldReturnReportCardForStudent()
    {
        var mockLogger = new Mock<ILogger<ReportCardController>>();
        var mockStudentClient = new Mock<IStudentClient>();
        mockStudentClient.Setup(s => s.GetStudentById(It.IsAny<int>()))
            .ReturnsAsync(new StudentDto()
        {
            FirstName = "First",
            LastName = "Last",
            Address = "Some Address"
        });
        var controller = new ReportCardController(mockLogger.Object, mockStudentClient.Object);

        var objectResult = (ObjectResult)(await controller.GetById(1));
        Assert.That(objectResult.StatusCode, Is.EqualTo(200));
        Assert.Pass();
    }
}