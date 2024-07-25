namespace McenterTest.Services.Requests.Models;

public class BearerToken
{
    public string Access_token { get; set; } = string.Empty;
    public int Expires_in { get; set; }
    public string Token_type { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
    public DateTime Expires_at { get; set; }
}