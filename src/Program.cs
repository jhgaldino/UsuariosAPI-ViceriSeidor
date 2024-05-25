using FluentValidation;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Services;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao container.
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "UsuariosAPI_ViceriSeidor", 
        Version = "v1",
        Description = "Uma API REST para um aplicativo de gerenciamento de usuários, com base no conceito CRUD (Create, Read, Update, Delete)"
    });
    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));

});
// Adiciona validadores ao container.
builder.Services.AddValidatorsFromAssemblies(new[] { typeof(Usuario).Assembly });

// Adiciona o DbContext ao container.
builder.Services.AddDbContext<UsuariosContext>(options =>
    options.UseInMemoryDatabase("Usuarios"));

var app = builder.Build();

// Requests na pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "UsuariosAPI_ViceriSeidor v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

