using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using GastosResidenciais.Api.Data;
using GastosResidenciais.Api.Middleware;
using GastosResidenciais.Api.Repositories;
using GastosResidenciais.Api.Services;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

var allowedOrigin = Environment.GetEnvironmentVariable("Cors__AllowedOrigin")
    ?? "http://localhost:5173";

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendVite", policy =>
    {
        policy.WithOrigins(allowedOrigin)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=gastos.db";

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ITransacaoRepository, TransacaoRepository>();
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ITransacaoService, TransacaoService>();
builder.Services.AddScoped<IRelatorioService, RelatorioService>();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var erros = context.ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault() ?? "Dados invalidos.";

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new { mensagem = erros });
        };
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Gerador de documento OpenAPI nativo do ASP.NET Core (.NET 9+)
builder.Services.AddOpenApi();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    // Expoe o documento OpenAPI JSON em /openapi/v1.json
    app.MapOpenApi();

    // Swagger UI em /swagger apontando para o documento gerado acima
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "GastosResidenciais API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("FrontendVite");
app.MapControllers();

app.Run();
