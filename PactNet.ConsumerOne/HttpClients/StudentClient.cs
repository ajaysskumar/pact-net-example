using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PactNet.ConsumerOne.HttpClients
{
    public interface IStudentClient
    {
        Task<StudentDto> GetStudentById(int studentId);
    }

    public class StudentClient : IStudentClient
    {
        private readonly HttpClient _httpClient;
        public StudentClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<StudentDto> GetStudentById(int studentId)
        {
            var studentResponse = await _httpClient.GetAsync($"https://localhost:5001/Student/{studentId}");
            if (studentResponse.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<StudentDto>(await studentResponse.Content.ReadAsStringAsync());
            }

            return null;
        }
    }
}