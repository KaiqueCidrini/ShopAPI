using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shoppin.AuthInterface;
using Shoppin.Data;
using Shoppin.Models;
using Shoppin.Service;

namespace Shoppin.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        public UserController(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }


        [HttpGet]
        [Route("")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await _context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        //[Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Post([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                //Força usuário ser sempre funcionario
                model.Role = "Funcionario";

                _context.Users.Add(model);
                await _context.SaveChangesAsync();
                model.Password = "";
                return model;
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel criar o usuário" });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(int id, [FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "Usuário não encontrado" });
            }
            try
            {
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return model;
                
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possivel atualizar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User user)
        {
            var result = await _authService.AuthenticateAsync(user.Username, user.Password);
            return Ok(result);
        }

    }
}