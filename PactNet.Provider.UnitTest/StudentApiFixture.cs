using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace PactNet.Provider.UnitTest
{
    public class StudentApiFixture : IDisposable
    {
        private readonly IHost server;
        public Uri ServerUri { get; }

        public StudentApiFixture()
        {
            ServerUri = new Uri("http://localhost:9226");
            server = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls(ServerUri.ToString());
                    webBuilder.UseStartup<TestStartup>();
                })
                .Build();
            server.Start();
        }

        public void Dispose()
        {
            server.Dispose();
        }
    }
}