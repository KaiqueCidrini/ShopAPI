using Microsoft.AspNetCore.Mvc;

namespace Shoppin.AuthInterface
{
    public interface IAuthService
    {
    Task<ActionResult<dynamic>> AuthenticateAsync(string username, string password);
    }
}