using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet.ConsumerOne.Models.Events;
using PactNet.Infrastructure.Outputters;
using PactNet.Matchers;
using PactNet.Provider.UnitTest;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;

namespace PactNet.ConsumerOne.UnitTest;

public class StudentConsumerEventTests
{
    private const string UuidRegex = "[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}";
    private readonly IMessagePactBuilderV3 _messagePact;
    private ITestOutputHelper OutputHelper { get; }

    public StudentConsumerEventTests(ITestOutputHelper output)
    {
        OutputHelper = output;

        IPactV3 v3 = Pact.V3("Result", "Student Event", new PactConfig
        {
            PactDir = "../../../../Pacts",
            DefaultJsonSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            },
            Outputters = new List<IOutput>
            {
                new XunitOutput(OutputHelper)
            },

            LogLevel = PactLogLevel.Information
        });

        _messagePact = v3.WithMessageInteractions();
    }

    [Fact]
    public void ReceivesStudentCreatedEvent()
    {
        _messagePact
            .ExpectsToReceive("a StudentCreatedEvent")
            .WithJsonContent(new
                {
                    StudentId = Match.Regex("2274a1f8-7d93-11ee-b962-0242ac120002",$"^{UuidRegex}$"),
                    StandardId = Match.Integer(2),
                    FirstName = Match.Regex("FirstName",$"^[a-zA-Z]{{3,25}}$$"),
                    LastName = Match.Regex("LastName",$"^[a-zA-Z]{{3,25}}$$")
                }
            )
            .Verify<StudentCreatedEvent>(message =>
                {
                    message.StudentId.Should().BeEquivalentTo("2274a1f8-7d93-11ee-b962-0242ac120002");
                    message.FirstName.Should().BeEquivalentTo("FirstName");
                    message.LastName.Should().BeEquivalentTo("LastName");
                    message.StandardId.Should().Be(2);
                }
            );
    }
}