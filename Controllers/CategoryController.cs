using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppin.Data;
using Shoppin.Models;

//Endpoint = URL
//https://localhost:5001/Categories
//http://localhost:5000
//https://meuapp.azurewebsites.com
[Route("categories")]
public class CategoryController : Controller
{
    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().ToListAsync();
        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context)
    {
        var categories = await context.Categories.AsNoTracking().FirstOrDefaultAsync();
        return Ok(categories);
    }

    [HttpPost]
    [Route("")]
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
            return Ok(new {message = "Categoria removida com sucesso!"});
        }
        catch(Exception)
        {
            return BadRequest(new {message = "Não foi possivel encontrar uma categoria"});
        }
    }
}