using System.Net;
using NUnit.Framework;
using PactNet.ConsumerOne.HttpClients;
using Xunit;
using Assert = NUnit.Framework.Assert;
using PactBrokerUtiliy = PactNet.ConsumerOne.UnitTest.Utilities.PactBrokerUtiliy;

namespace PactNet.ConsumerOne.UnitTest
{
    public class ReportCardApiPactFlowTest
    {
        private readonly IPactBuilderV3 _pactBuilder;
        private const string ValidStudentId = "067a50c5-0b23-485e-b018-17c66b2422ff";
        
        public ReportCardApiPactFlowTest()
        {
            var pactDir =
                $"{Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName}{Path.DirectorySeparatorChar}pacts";

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
                .Given($"There is student with id {ValidStudentId}")
                .WithRequest(HttpMethod.Get, $"/students/{ValidStudentId}")
                .WithHeader("Accept", "application/json")
                .WillRespond()
                .WithStatus(HttpStatusCode.OK)
                .WithHeader("Content-Type", "application/json; charset=utf-8")
                .WithJsonBody(new
                {
                    firstName = "Raju",
                    lastName = "Rastogi",
                    address = "Delhi",
                    id = ValidStudentId
                });

            await _pactBuilder.VerifyAsync(async ctx =>
            {
                // Act
                var client = new StudentClient(ctx.MockServerUri);
                var studentDto = await client.GetStudentById(ValidStudentId);

                // Assert
                Assert.That(studentDto.Id, Is.EqualTo(ValidStudentId));
            });
            
            await UploadPactContract();
        }

        [Fact]
        public async Task Get_Student_When_The_StudentId_Is_Invalid()
        {
            // Arrange
            _pactBuilder
                .UponReceiving("A GET request to retrieve the student with invalid student id")
                .Given("There is student is at least one valid student present")
                .WithRequest(HttpMethod.Get, "/students/some-invalid-id")
                .WithHeader("Accept", "application/json")
                .WillRespond()
                .WithStatus(HttpStatusCode.NoContent);
 
            _pactBuilder.Verify(ctx =>
            {
                // Act
                var client = new StudentClient(ctx.MockServerUri);
                Assert.ThrowsAsync<Exception>(async () => await client.GetStudentById("some-invalid-id"));
            });
            
            await UploadPactContract();
        }
        
        private static async Task UploadPactContract()
        {
            var pactPath = Path.Combine("..",
                "..",
                "..",
                "..",
                "pacts",
                "ConsumerOne-Student API.json");

            var contractJson = File.ReadAllText(pactPath);
            var statusCode = await PactBrokerUtiliy.PublishPactContract(
                "PACT_FLOW_BASE_URI",
                "ConsumerOne",
                "Student API",
                contractJson,
                "PACT_FLOW_TOKEN",
                Guid.NewGuid().ToString());
        }
    }
}