using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var qdrant = builder.AddQdrant("qdrant")
                 .WithDataVolume();

var storage = builder.AddAzureStorage("Storage");

if (builder.Environment.IsDevelopment())
{
    storage.RunAsEmulator(c => c.WithImageTag("3.33.0").WithDataVolume());
}

var blobs = storage.AddBlobs("BlobConnection");

var ollama = builder.AddOllama(modelName: "llama3.2").WithVolume("ollama");

// create persistent password in user secrets : dotnet user-secrets set Parameters:postgresql-password <password>
var sqlPassword = builder.AddParameter("postgresql-password", secret: true);
var postgres = builder
    .AddPostgres("postgresql", password: sqlPassword, port: 65534)
    .WithEnvironment("POSTGRES_DB", "elysio")
    .WithOtlpExporter()
    .WithDataVolume()
    .WithPgAdmin()
    .WithPgWeb()
.AddDatabase("postgresdb", "elysio");

var api = builder.AddProject<Projects.Elysio_API>("elysio-api")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(qdrant)
    .WaitFor(qdrant)
    .WithReference(blobs)
    .WaitFor(blobs)
    .WithReference(ollama)
    .WaitFor(ollama);

var webapp = builder.AddNpmApp("webapp", @"..\..\..\10-Client\elysio.crew", "dev")
    .WithEnvironment("CLIENT_ID", "e7458213-536e-4d2f-a301-daa5327442e4")
    .WithEnvironment("TENANT_ID", "37f626ab-77a8-4087-9c44-ef395de90a98")
    .WithEnvironment("API_SCOPE", "api://C2S-AOAI/api.all")
    .WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .PublishAsDockerFile()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();