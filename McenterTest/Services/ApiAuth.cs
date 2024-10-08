using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;
using McenterTest.Utilities;
using McenterTest.Services;
using McenterTest.Services.Requests.Models;

namespace McenterTest.Models;

/// <summary>
/// The <see cref="ApiAuth"/> class is responsible for handling the extraction of credential file contents 
/// and generation of a bearer token for authentication purposes.
/// </summary>
/// <remarks>
/// This class provides methods to:
/// <list type="bullet">
/// <item>
/// <description>Extract the contents of a token file specified by an environment variable.</description>
/// </item>
/// <item>
/// <description>Send HTTP requests with the extracted token information.</description>
/// </item>
/// <item>
/// <description>Generate and validate bearer tokens used for authorization.</description>
/// </item>
/// </list>
/// </remarks>
public class ApiAuth
{
    private static string? baseUrl;
    private static string? opennessTokenFilePath = "../../../OpenAPIClientCredentials.json";
    private static TokenFile? tokenFile;
    private static BearerToken? bearerToken;
    private static JsonSerializerOptions serializerOptions;
    private static LogWriter logWriter = LogWriter.Instance;

    /// <summary>
    /// Extracts the contents of the token file specified by the environment variable <c>opennessTokenFilePath</c>.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Checks if the <c>opennessTokenFilePath</c> is set. If not, it throws an <see cref="Exception"/>.</description>
    /// </item>
    /// <item>
    /// <description>Initializes the <c>serializerOptions</c> with case-insensitive property names.</description>
    /// </item>
    /// <item>
    /// <description>Reads the contents of the token file specified by <c>opennessTokenFilePath</c>.</description>
    /// </item>
    /// <item>
    /// <description>Deserializes the token file content into a <see cref="TokenFile"/> object.</description>
    /// </item>
    /// <item>
    /// <description>Extracts the base URL from the deserialized token file and sets it in the <see cref="HttpClientFactory"/>.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="Exception">Thrown when <c>opennessTokenFilePath</c> is not set or when the token file is not available.</exception>
    private static void extractFileContents()
    {
        // check if token path is set
        if (string.IsNullOrWhiteSpace(opennessTokenFilePath))
        {
            throw new Exception("Please set the token file path in your environment");
        }

        serializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        // read content from token file to extract the necessary stuff
        string credentialFileText = File.ReadAllText(opennessTokenFilePath);
        
        // deserialize token file content
        tokenFile = JsonSerializer.Deserialize<TokenFile>(credentialFileText, serializerOptions);
            
        //extract baseURL from openness token
        var baseURI = (tokenFile != null) ? new Uri(tokenFile.IdentityServiceUrl): throw new Exception($"Openness token not available");
        baseUrl =  baseURI.Scheme + "://" + baseURI.Authority;

        HttpClientFactory.setBaseUrl(baseUrl);

        logWriter.LogWithTimestamp($"Setting BaseUrl {HttpClientFactory.getBaseUrl()}", LogWriter.LogLevel.Debug);
    }

    /// <summary>
    /// Checks the validity of the bearer token.
    /// </summary>
    /// <returns><c>true</c> if the token needs to be generated or renewed; otherwise, <c>false</c>.</returns>
    /// <remarks>
    /// This method performs the following checks:
    /// <list type="number">
    /// <item>
    /// <description>Verifies if the <see cref="bearerToken"/> is <c>null</c> or if it will expire in less than 5 minutes.</description>
    /// </item>
    /// <item>
    /// <description>Throws an <see cref="Exception"/> if the <see cref="tokenFile"/> is <c>null</c>.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="Exception">Thrown when the <see cref="tokenFile"/> is <c>null</c>.</exception>
    private static bool checkBearerToken()
    {
        // check if token exists and if it is valid for less than 5 minutes
        if (bearerToken == null ||
            bearerToken.Expires_at < DateTime.Now.AddMinutes(5))
        {
            return true;
        }

        // throw exception if opennessToken is not existing
        if (tokenFile == null)
        {
            throw new Exception("tokenFile Null: Tried to read the token file when it was null.");
        }

        return false;
    }
    
    /// <summary>
    /// Generates a bearer token by sending a request to the token endpoint.
    /// </summary>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Checks if a new token needs to be generated by verifying the current token's expiration time.</description>
    /// </item>
    /// <item>
    /// <description>If a new token is needed, prepares the request URL and body with client credentials.</description>
    /// </item>
    /// <item>
    /// <description>Sends the request to the token endpoint and receives the response.</description>
    /// </item>
    /// <item>
    /// <description>Deserializes the response to obtain the bearer token.</description>
    /// </item>
    /// <item>
    /// <description>Sets the expiration date of the token based on the response.</description>
    /// </item>
    /// <item>
    /// <description>Sets the bearer token as the default authorization header for all requests.</description>
    /// </item>
    /// </list>
    /// If the token does not need to be generated, it logs a message indicating the token is still valid.
    /// </remarks>
    /// <exception cref="Exception">Thrown when an error occurs during token generation or deserialization.</exception>
    private static void generateBearerToken()
    {
        // check if a token needs to be generated
        if (checkBearerToken())
        {
            try
            {
                // endpoint to get the bearer token
                string requestUrl = tokenFile.IdentityServiceUrl + "/connect/token";

                // build content body with client id and other content from ClientCredentials tokenFile
                var requestBody = new List<KeyValuePair<string, string>>
                {
                    new("grant_type", "ClientAuthGrantType"),
                    new("client_id", tokenFile.IdentityServer4Client),
                    new("ClientAuthPayload", JsonSerializer.Serialize(tokenFile, serializerOptions)),
                };
                
                // Send request to endpoint with body as message
                HttpResponseMessage? response = HttpRequests.getBearerToken(requestUrl, HttpMethod.Post, requestBody);
                string responseText = response.Content.ReadAsStringAsync().Result;

                // deserialize bearer token
                bearerToken = JsonSerializer.Deserialize<BearerToken>(responseText, serializerOptions);
                if (bearerToken == null)
                {
                    // Display popup for acknowledgement
                    MessageBox.Show($"Bearer token could not be deserialized!\nSee logs", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                    logWriter.LogWithTimestamp($"Bearer token is null. Token could not be deserialized from '{responseText}'", LogWriter.LogLevel.Error);
                    throw new Exception($"Error Occurred: Bearer token could not be deserialized from '{responseText}'");
                }

                // set expiration date based on the timestamp of the response and the "expires_in" value
                bearerToken.Expires_at = (bearerToken != null && response.Headers.Date != null)
                    ? response.Headers.Date.Value.ToLocalTime().AddSeconds(bearerToken.Expires_in).DateTime : DateTime.Now;

                // set bearer token as default header for all requests
                HttpClientFactory.GetHttpClient().DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", bearerToken?.Access_token);

                logWriter.LogWithTimestamp($"Bearer Token Generated: Bearer token is valid until, {bearerToken?.Expires_at}", LogWriter.LogLevel.Info);
            }
            catch (Exception e)
            {
                // Display popup for acknowledgement
                MessageBox.Show("An error occurred when generating the bearer token!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                logWriter.LogWithTimestamp($"Error Generating Token: {e}", LogWriter.LogLevel.Error);
            }
            
        }
        else
        {
            logWriter.LogWithTimestamp($"Token does not need to be generated, already exists with enough time.", LogWriter.LogLevel.Info);
        }
    }

    /// <summary>
    /// Gets the bearer token, generating it if necessary.
    /// </summary>
    /// <returns>The bearer token.</returns>
    /// <remarks>
    /// This method performs the following steps:
    /// <list type="number">
    /// <item>
    /// <description>Extracts the contents of the token file.</description>
    /// </item>
    /// <item>
    /// <description>Generates a bearer token if one is not already valid.</description>
    /// </item>
    /// <item>
    /// <description>Returns the access token part of the bearer token.</description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="Exception">Thrown when an error occurs during the process of getting the bearer token.</exception>
    public static string? getBearerToken()
    {
        try
        {
            extractFileContents();
            generateBearerToken();

            return bearerToken?.Access_token;
        }
        catch (Exception e)
        {
            logWriter.LogWithTimestamp($"An error occurred when getting the bearer token. {e}", LogWriter.LogLevel.Error);
            throw new Exception($"Error Occurred: An error occurred when getting the bearer token. {e}");
        }
    }
}