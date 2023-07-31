using FormGeneratorAPI.Authentication.Entities;

namespace FormGeneratorAPI.Authentication;

public interface IJwtManagerRepository
{
    JwtToken Authenticate(User user); 
}