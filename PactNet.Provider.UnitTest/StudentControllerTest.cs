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
        
        [Fact]
        public void Ensure_StudentApi_Honours_Pact_With_ConsumerOne_Using_PactFlow()
        {
            // Arrange
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput>
                {
                    new XunitOutput(_output),
                },
                LogLevel = PactLogLevel.Information
            };

            var pactFlowBaseUri = Environment.GetEnvironmentVariable("PACT_FLOW_BASE_URL"); // For testing purposes, you may even hardcode this value
            var pactFlowToken = Environment.GetEnvironmentVariable("PACT_FLOW_TOKEN"); // For testing purposes, you may even hardcode this value

            // Act // Assert
            IPactVerifier pactVerifier = new PactVerifier(config);
            pactVerifier
                .ServiceProvider("Student API", _fixture.ServerUri)
                .WithPactBrokerSource(new Uri(pactFlowBaseUri), configure =>
                {
                    configure.TokenAuthentication(pactFlowToken);
                    configure.PublishResults(true, "1.0.0"); // Any version
                })
                .WithProviderStateUrl(new Uri(_fixture.ServerUri, "/provider-states"))
                .Verify();
        }
    }
}

