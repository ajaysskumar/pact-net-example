using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace PactNet.ConsumerOne.UnitTest.Utilities;

public abstract class PactBrokerUtiliy
{
    private const string PactSubUrl = "{0}/pacts/provider/{1}/consumer/{2}/version/{3}";

    public static async Task<HttpStatusCode> PublishPactContract(string pactFlowBaseUri,string consumerName, string providerName, string contractJson, string accessToken, string consumerVersion = "")
    {
        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken);
        var fullUrl = string.Format(PactSubUrl, pactFlowBaseUri, providerName, consumerName, string.IsNullOrEmpty(consumerVersion) ? Guid.NewGuid() : consumerVersion);

        var response =
            await httpClient.PutAsync(fullUrl, new StringContent(contractJson, Encoding.UTF8, "application/json"));
        var responseContent = await response.Content.ReadAsStringAsync();
        return response.StatusCode;
    }
}