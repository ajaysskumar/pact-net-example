using System.Net;
using NUnit.Framework;
using PactNet.ConsumerOne.HttpClients;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace PactNet.ConsumerOne.UnitTest
{
    public class ReportCardControllerTest
    {
        private readonly IPactBuilderV3 pactBuilder;

        public ReportCardControllerTest()
        {
            // Use default pact directory ..\..\pacts and default log
            // directory ..\..\logs
            // var pact = Pact.V3("Student API Consumer", "Student API", new PactConfig());

            var pactDir =
                $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts";


            Console.WriteLine($"PACT_DIR: {pactDir}");
            // or specify custom log and pact directories
            var pact = Pact.V3("ConsumerOne", "Student API", new PactConfig
            {
                PactDir = pactDir,
                LogLevel = PactLogLevel.Debug
            });

            // Initialize Rust backend
            this.pactBuilder = pact.WithHttpInteractions();
        }

        [Fact]
        public async Task GetStudent_WhenTheTesterStudentExists_ReturnsTheStudent()
        {
            // Arrange
            this.pactBuilder
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

            await this.pactBuilder.VerifyAsync(async ctx =>
            {
                // Act
                var client = new StudentClient(ctx.MockServerUri);
                var studentDto = await client.GetStudentById(1);

                // Assert
                Assert.That(studentDto.Id, Is.EqualTo(1));
            });
        }

        [Fact]
        public async Task GetSomething_WhenTheTesterExists_ReturnsTheStudent()
        {
            // Arrange
            var pactDir =
                $"{Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}{Path.DirectorySeparatorChar}pacts";

            var pact = Pact.V3("ConsumerOne", "Something API", new PactConfig()
            {
                PactDir = pactDir,
                LogLevel = PactLogLevel.Debug
            });

            // Initialize Rust backend
            var localPactBuilder = pact.WithHttpInteractions();

            localPactBuilder
                .UponReceiving("A GET request to retrieve the something")
                .Given("There is a something with id 'tester'")
                .WithRequest(HttpMethod.Get, "/somethings/tester")
                .WithHeader("Accept", "application/json")
                .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new
                {
                    firstName = "Totally",
                    lastName = "Awesome",
                    id = "tester"
                });

            await localPactBuilder.VerifyAsync(async ctx =>
            {
                // Act
                var client = new SomethingClient(ctx.MockServerUri);
                var studentDto = await client.GetSomethingById("tester");

                // Assert
                Assert.That(studentDto.Id, Is.EqualTo("tester"));
            });
        }
    }
}