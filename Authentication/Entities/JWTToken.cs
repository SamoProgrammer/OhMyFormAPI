namespace FormGeneratorAPI.Authentication.Entities;

public class JwtToken
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }
}