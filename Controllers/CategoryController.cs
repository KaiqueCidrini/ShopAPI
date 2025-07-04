using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppin.Data;
using Shoppin.Models;

//Endpoint = URL
//https://localhost:5001/Categories
//http://localhost:5000
//https://meuapp.azurewebsites.com
[Route("v1/categories")]
public class CategoryController : ControllerBase
{
    [HttpGet]
    [Route("")]
    [AllowAnonymous]
    [ResponseCache(VaryByHeader ="User-Agent", Location = ResponseCacheLocation.Any, Duration = 30)]
    //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] - Para dizer que o metodo não tem cache caso a program esteja cacheando tudo.
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices] DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().FirstOrDefaultAsync();
        return Ok(categories);
    }

    [HttpPost]
    [Route("")]
    [Authorize(Roles = "Funcionario")]
    public async Task<ActionResult<Category>> Post(
        [FromBody] Category model,
        [FromServices] DataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel criar a Categoria." });
        }

    }

    [HttpPut]
    [Route("{id:int}")]
    [Authorize(Roles = "Funcionario")]
    
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model, [FromServices] DataContext context)
    {
        //Verifica se os dados são válidos.
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return model;
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    [Authorize(Roles = "Funcionario")]
    public async Task<ActionResult<Category>> Delete(
        int id,
        [FromServices] DataContext context
    )
    {
        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
            return NotFound(new { message = "Categoria não encontrada" });
        try
        {
            context.Categories.Remove(category);
            await context.SaveChangesAsync();
            return Ok(new { message = "Categoria removida com sucesso!" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel encontrar uma categoria" });
        }
    }
}