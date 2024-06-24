using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PactNet.Provider.Models;
using PactNet.Provider.Repository;

namespace PactNet.Provider.UnitTest
{

    public class ProviderStateMiddleware
    {
        private readonly IDictionary<string, Action> _providerStates;
        private readonly RequestDelegate _next;
        private readonly IStudentRepo _studentRepo;

        public ProviderStateMiddleware(RequestDelegate next, IStudentRepo studentRepo)
        {
            _next = next;
            _studentRepo = studentRepo;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is student with id 067a50c5-0b23-485e-b018-17c66b2422ff",
                    AddStudentIfItDoesntExist
                },
                {
                    "There is student is at least one valid student present",
                    AddAnotherStudent
                }
            };
        }

        private void AddStudentIfItDoesntExist()
        {
            _studentRepo.AddStudent(new Student
            {
                Id = "067a50c5-0b23-485e-b018-17c66b2422ff",
                FirstName = "Raju",
                LastName = "Rastogi",
                Address = "Delhi"
            });
        }
        
        private void AddAnotherStudent()
        {
            _studentRepo.AddStudent(new Student
            {
                Id = "477a74aa-986f-4fc9-b48a-8aea50d19258",
                FirstName = "John",
                LastName = "Doe",
                Address = "Agra"
            });
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path
                    .Value?.StartsWith("/provider-states") ?? false)
            {
                await _next.Invoke(context);
                return;
            }

            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method == HttpMethod.Post.ToString()
                && context.Request.Body != null)
            {
                string jsonRequestBody;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (!string.IsNullOrEmpty(providerState?.State))
                {
                    _providerStates[providerState.State].Invoke();
                }

                await context.Response.WriteAsync(string.Empty);
            }
        }
    }
}