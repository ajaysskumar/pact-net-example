using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PactNet.Provider.UnitTest
{
    public class StudentApiFixture : IDisposable
    {
        private readonly IHost _server;
        public Uri ServerUri { get; }

        public StudentApiFixture()
        {
            ServerUri = new Uri("http://localhost:9226");
            _server = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(ServerUri.ToString());
                    webBuilder.UseStartup<TestStartup>();
                })
                .Build();
            _server.Start();
        }

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}