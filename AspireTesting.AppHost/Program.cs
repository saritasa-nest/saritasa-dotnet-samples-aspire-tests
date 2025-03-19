using AspireTesting.AppHost.Integrations.MailHog;

var builder = DistributedApplication.CreateBuilder(args);

var databaseConfiguration = builder.AddPostgres(name: "database", port: 5433)
    .WithEnvironment("POSTGRES_DB", "Testing");

databaseConfiguration = SetupData(args, databaseConfiguration);

var database = databaseConfiguration.AddDatabase(name: "AppDatabase", databaseName: "Testing");

var mailhog = builder.AddMailHog("Smtp")
    .WithPorts(httpPort: 8025, smtpPort: 1025)
    .FromAddress("test@example.com")
    .UseSsl();

var api = builder.AddProject<Projects.AspireTesting>(Constants.ApiResourceName)
    .WithReference(database)
    .WithReference(mailhog)
    .WaitFor(database)
    .WaitFor(mailhog);

var frontend = builder.AddProject<Projects.AspireTesting_Client>("frontend")
    .WithReference(api);

builder.Build().Run();

IResourceBuilder<PostgresServerResource> SetupData(string[] args, 
    IResourceBuilder<PostgresServerResource> databaseConfiguration)
{
    if (args.Any(arg => arg == Constants.IntegrationTest))
    {
        databaseConfiguration = databaseConfiguration
            .WithBindMount("./data", "/docker-entrypoint-initdb.d");
    }
    else
    {
        databaseConfiguration = databaseConfiguration.WithDataVolume();
    }

    return databaseConfiguration;
}

public class Constants 
{
    public const string ApiResourceName = "api";

    public const string IntegrationTest = "IntegrationTest";
}