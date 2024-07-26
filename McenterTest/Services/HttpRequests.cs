using System.Net.Http;
using System.Net.Http.Headers;

namespace McenterTest.Services;

public class HttpRequests
{
    
    /// <summary>
    /// This method sends out an http request and should respond with
    /// and http response that can be displayed for the user
    /// </summary>
    /// <param name="urlExtension">the extension to reach the endpoint, '/mmr/api/.../endpoint'</param>
    /// <param name="httpMethod">type of request being sent, get/post/patch</param>
    /// <param name="requestBody">the body of the request, should be formatted similar to JSON</param>
    public HttpResponseMessage? httpRequest(string urlExtension, HttpMethod httpMethod, List<KeyValuePair<string, string>> requestBody)
    {
        // concatenate the baseUrl and the extension to get the endpoint
        string requestUrl = HttpClientFactory.getBaseUrl() + urlExtension;
        
        // create request for non Get messages
        HttpRequestMessage request = new(httpMethod, requestUrl);
        
        // create response variable to capture http response
        HttpResponseMessage? response = null;
        
        // send requests
        if (httpMethod.Equals(HttpMethod.Get))
        {
            // get messages only require the url endpoint
            response = HttpClientFactory.GetHttpClient().GetAsync(requestUrl).Result;
        }
        else
        {
            // non get messages require the endpoint and a body
            response = HttpClientFactory.GetHttpClient().SendAsync(request).Result;
        }
        
        if (response == null)
        {
            throw new SystemException("Error Occurred: Response from HttpClient is null");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new SystemException($"Request returned with error: {requestUrl} - {response.StatusCode} - {response.Content.ReadAsStringAsync().Result}");
        }
        
        // return response
        return response;
    }
}