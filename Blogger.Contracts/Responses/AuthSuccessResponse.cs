namespace Blogger.Contracts.Responses;

public class AuthSuccessResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
