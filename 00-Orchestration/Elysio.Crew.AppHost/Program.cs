var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Elysio_API>("elysio-api");

builder.Build().Run();
