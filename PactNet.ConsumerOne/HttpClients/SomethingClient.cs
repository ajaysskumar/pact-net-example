using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PactNet.ConsumerOne.HttpClients
{
    public interface ISomethingClient
    {
        Task<StudentDto> GetStudentById(int studentId);
    }

    // public class StudentClient : IStudentClient
    // {
    //     private readonly HttpClient _httpClient;
    //     public StudentClient(Uri uri = null)
    //     {
    //         _httpClient = new HttpClient()
    //         {
    //             BaseAddress = uri ?? new Uri("https://localhost:5001")
    //         };
    //     }
    //
    //     public async Task<StudentDto> GetStudentById(int studentId)
    //     {
    //         _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //         var studentResponse = await _httpClient.GetAsync($"/Student/{studentId}");
    //         return studentResponse.IsSuccessStatusCode ? JsonConvert.DeserializeObject<StudentDto>(await studentResponse.Content.ReadAsStringAsync()) : null;
    //     }
    // }
    
    public class SomethingClient
    {
        private readonly HttpClient client;

        public SomethingClient(Uri baseUri = null)
        {
            this.client = new HttpClient { BaseAddress = baseUri ?? new Uri("https://localhost:5001/") };
        }

        public async Task<Something> GetSomethingById(string id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/somethings/{id}");
            request.Headers.Add("Accept", "application/json");

            var response = await this.client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            string reasonPhrase = response.ReasonPhrase;

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<Something>(content)
                    : null;
            }

            throw new Exception(reasonPhrase);
        }
        
        public async Task<List<StudentDto>> GetAllStudents()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"/students");
            request.Headers.Add("Accept", "application/json");

            var response = await this.client.SendAsync(request);

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