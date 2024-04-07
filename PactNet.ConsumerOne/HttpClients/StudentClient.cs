using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PactNet.ConsumerOne.Models;

namespace PactNet.ConsumerOne.HttpClients
{
    public interface IStudentClient
    {
        Task<StudentDto> GetStudentById(int studentId);
    }
    
    public class StudentClient: IStudentClient
    {
        private readonly HttpClient _client;

        public StudentClient(Uri baseUri = null)
        {
            this._client = new HttpClient { BaseAddress = baseUri ?? new Uri("https://localhost:5001/") };
        }

        public async Task<StudentDto> GetStudentById(int studentId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/students/" + studentId);
            request.Headers.Add("Accept", "application/json");

            var response = await this._client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<StudentDto>(content)
                    : null;
            }
            
            if (status == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new Exception(reasonPhrase);
        }
        
        public async Task<List<StudentDto>> GetAllStudents()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/students");
            request.Headers.Add("Accept", "application/json");

            var response = await this._client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<List<StudentDto>>(content)
                    : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}