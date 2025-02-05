#pragma warning disable SKEXP0070

using Elysio.API.Routes;
using Elysio.Data;
using Elysio.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.SemanticKernel;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add logging services
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    // Add other logging providers as needed
});

var isDevelopment = builder.Environment.IsDevelopment();

// Add service defaults and other services
builder.AddServiceDefaults();
builder.AddQdrantClient("qdrant");
builder.AddAzureBlobClient("BlobConnection");
builder.AddNpgsqlDbContext<ApplicationDbContext>(
    "postgresdb",
    null,
    options => options.UseNpgsql(x => x.MigrationsAssembly("Elysio.Data"))
);

string connectionString = builder.Configuration.GetConnectionString("Ollama") ?? "";
builder.Services.AddOllamaChatCompletion("llama3.2:1b", new Uri(connectionString));

// Authentication and Authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();

var (policy, adminPolicy) = builder.Services.AddGroupPolicyExtension(builder.Configuration);

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (isDevelopment)
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapAgentsEndpoints(policy);
app.MapUsersEndpoints(policy);
app.MapConversationsEndpoints(policy);
app.MapMessagesEndpoints(policy);
app.MapRoomsEndpoints(policy);

app.Run();