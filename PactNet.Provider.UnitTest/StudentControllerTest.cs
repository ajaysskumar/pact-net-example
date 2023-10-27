using System;
using System.Collections.Generic;
using System.IO;
using PactNet.Infrastructure.Outputters;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;

namespace PactNet.Provider.UnitTest
{
    public class StudentControllerTest : IClassFixture<StudentApiFixture>
    {
        private readonly StudentApiFixture _fixture;
        private readonly ITestOutputHelper _output;

        public StudentControllerTest(StudentApiFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
    
        [Fact]
        public void Ensure_StudentApi_Honours_Pact_With_ConsumerOne()
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    // NOTE: PactNet defaults to a ConsoleOutput, however
                    // xUnit 2 does not capture the console output, so this
                    // sample creates a custom xUnit outputter. You will
                    // have to do the same in xUnit projects.
                    new XunitOutput(_output),
                },
                LogLevel = PactLogLevel.Information
            };

            string pactPath = Path.Combine("..",
                "..",
                "..",
                "..",
                "pacts",
                "ConsumerOne-Student API.json");

            // Act // Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ServiceProvider("Student API", _fixture.ServerUri)
                .WithFileSource(new FileInfo(pactPath))
                .WithProviderStateUrl(new Uri(_fixture.ServerUri, "/provider-states"))
                .Verify();
        }
    }
}

