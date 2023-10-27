using System.Net;
using NUnit.Framework;
using PactNet.ConsumerOne.HttpClients;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace PactNet.ConsumerOne.UnitTest
{
    public class ReportCardControllerTest
    {
        private readonly IPactBuilderV3 _pactBuilder;

        public ReportCardControllerTest()
        {
            var pactDir =
                $"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent.Parent.Parent?.FullName}{Path.DirectorySeparatorChar}pacts";

            Console.WriteLine($"PACT_DIR: {pactDir}");
            // or specify custom log and pact directories
            var pact = Pact.V3("ConsumerOne", "Student API", new PactConfig
            {
                PactDir = pactDir,
                LogLevel = PactLogLevel.Debug
            });

            // Initialize Rust backend
            _pactBuilder = pact.WithHttpInteractions();
        }

        [Fact]
        public async Task Get_Student_When_The_Student_With_Id_1_Exists()
        {
            // Arrange
            _pactBuilder
                .UponReceiving("A GET request to retrieve the student")
                .Given("There is student with id 1")
                .WithRequest(HttpMethod.Get, "/students/" + 1)
                .WithHeader("Accept", "application/json")
                .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new
                {
                    firstName = "Raju",
                    lastName = "Rastogi",
                    address = "Delhi",
                    id = 1
                });

            await _pactBuilder.VerifyAsync(async ctx =>
            {
                // Act
                var client = new StudentClient(ctx.MockServerUri);
                var studentDto = await client.GetStudentById(1);

                // Assert
                Assert.That(studentDto.Id, Is.EqualTo(1));
            });
        }
    }
}