using Microsoft.AspNetCore.Identity;

namespace NivelaService.Repository.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
