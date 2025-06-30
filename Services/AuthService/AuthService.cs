using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppin.Data;
using Shoppin.AuthInterface;
using Shoppin.Service;

namespace Shoppin.AuthService
{
  
    
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        public AuthService(DataContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<dynamic>> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.AsNoTracking().Where(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();
            if (user == null)
            {
                return "Nome de usuario ou senha incorretos.";
            }

            var token = TokenService.GenerateToken(user);
            return new
            {
                user = user,
                token = token
            };

        }
    }
}