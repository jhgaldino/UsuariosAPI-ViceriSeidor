using FluentValidation;
using UsuariosAPI_ViceriSeidor.src.Data;
using UsuariosAPI_ViceriSeidor.src.Models;
using UsuariosAPI_ViceriSeidor.src.Services;
using UsuariosAPI_ViceriSeidor.src.Inferfaces;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Net;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics;


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
builder.Services.AddHealthChecks();
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
// Verificando a saúde da aplicação
app.MapHealthChecks(
    "/health",
    new HealthCheckOptions()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.UseExceptionHandler(appError=>
{
    appError.Run(async context => {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";

        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
    });
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

