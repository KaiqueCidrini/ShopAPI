using Microsoft.EntityFrameworkCore;
using Shoppin.Data;

var builder = WebApplication.CreateBuilder(args);

// Configuração de serviços (equivalente ao ConfigureServices do Startup.cs)
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
    opt.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));
builder.Services.AddScoped<DataContext>();

// Configuração do OpenAPI/Swagger (opcional)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Pipeline de requisições HTTP (equivalente ao Configure do Startup.cs)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

// Configuração de endpoints
app.MapControllers();

app.Run();