var builder = DistributedApplication.CreateBuilder(args);

var ollama = builder.AddOllama(modelName: "llama3.2");

var sqlPassword = builder.AddParameter("postgresql-password", secret: true);
var postgres = builder
    .AddPostgres("postgresql", password: sqlPassword, port: 65534)
    .WithEnvironment("POSTGRES_DB", "elysio")
    .WithOtlpExporter()
    .WithDataVolume()
    // .WithPgAdmin()
    .WithPgWeb()
.AddDatabase("postgresdb", "elysio");

var api = builder.AddProject<Projects.Elysio_API>("elysio-api")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithReference(ollama)
    .WaitFor(ollama);

var webapp = builder.AddNpmApp("webapp", @"..\..\10-Client\elysio.crew.front", "dev")
    .WithHttpEndpoint(port: 58300, env: "PORT")
    .WithExternalHttpEndpoints()
    .WithOtlpExporter()
    .PublishAsDockerFile()
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();