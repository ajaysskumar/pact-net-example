using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet.Infrastructure.Outputters;
using PactNet.Provider.Models.Events;
using PactNet.Provider.Shared;
using PactNet.Verifier;
using Xunit;
using Xunit.Abstractions;

namespace PactNet.Provider.UnitTest;

public class StudentProviderEventTests : IDisposable
{
    private readonly PactVerifier _pactVerifier;
    
    private ITestOutputHelper OutputHelper { get; }
    
    public StudentProviderEventTests(ITestOutputHelper output)
    {
        OutputHelper = output;
        
        var config = new PactVerifierConfig
        {
            // NOTE: We default to using a ConsoleOutput,
            // however xUnit 2 does not capture the console output,
            // so a custom outputter is required.
            Outputters = new List<IOutput>
            {
                new XunitOutput(OutputHelper)
            },

            LogLevel = PactNet.PactLogLevel.Trace
        };

        _pactVerifier = new PactVerifier(config);
    }
    
    public void Dispose()
    {
        // make sure you dispose the verifier to stop the internal messaging server
        GC.SuppressFinalize(this);
        _pactVerifier.Dispose();
    }
    
    
    [Fact]
    public void EnsureProviderHonoursPactWithConsumer()
    {
        FileInfo pactFile = new("../../../../Pacts/Result-Student Event.json");
        var defaultSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        _pactVerifier
            .MessagingProvider("Student Provider", defaultSettings)
            .WithProviderMessages(scenarios =>
            {
                scenarios.Add("a StudentCreatedEvent", builder =>
                {
                    builder
                        .WithMetadata(new
                        {
                            contentType = "application/json"
                        })
                        .WithContent(() => new StudentCreatedEvent()
                        {
                            StudentId = "2274a1f8-7d93-11ee-b962-0242ac120002",
                            Address = "Some Address",
                            Gender = "Male",
                            StandardId = Standard.Standard2,
                            FirstName = "FirstName",
                            LastName = "LastName"
                        });
                });
            })
            .WithFileSource(pactFile).Verify();
    }
}