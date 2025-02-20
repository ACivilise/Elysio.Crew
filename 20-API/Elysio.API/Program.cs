#pragma warning disable SKEXP0070

using Elsio.Crew.Domain;
using Elysio.API.Routes;
using Elysio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add logging services
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
});

var isDevelopment = builder.Environment.IsDevelopment();

// Add service defaults and other services
builder.AddServiceDefaults();
builder.AddNpgsqlDbContext<ApplicationDbContext>(
    "postgresdb",
    null,
    options => options.UseNpgsql(x => x.MigrationsAssembly("Elysio.Data"))
);
builder.Services.MigrateDb();

string connectionString = builder.Configuration.GetConnectionString("Ollama") ?? "";
builder.Services.AddOllamaChatCompletion("llama3.2", new Uri($"{connectionString}/v1"));
builder.Services.AddKernel();
builder.Services.AddCoreServices();
builder.Services.AddDomainServices();

builder.Services.AddOpenApi();
builder.Services.AddCors();

var app = builder.Build();  // Mise en place de la chaine de traitement des requêtes HTTP

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.Logger.LogInformation("Environment is Production, applying security headers");
    app.UseSecurityHeaders();
}

app.UseHttpsRedirection();

app.UseCors(static builder =>
    builder.AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin());

app.MapAgentsEndpoints()
    .MapConversationsEndpoints()
    .MapMessagesEndpoints()
    .MapRoomsEndpoints();

app.Run();