using Aspire.Hosting;
using AspireTesting.IntegrationTests.Abstractions;
using AspireTesting.IntegrationTests.Implementations;

namespace AspireTesting.IntegrationTests.Api;

/// <summary>
/// Setups API resource.
/// </summary>
public sealed class ApiFixture : IAsyncLifetime
{
    private DistributedApplication app;

    /// <summary>
    /// Http client that interacts with API resource.
    /// </summary>
    public IApiClient ApiClient { get; private set; } = null!;

    /// <inheritdoc/>
    public async Task InitializeAsync()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.AspireTesting_AppHost>([Constants.IntegrationTest]);
        appHost.Configuration["DcpPublisher:RandomizePorts"] = "false";

        appHost.Services.ConfigureHttpClientDefaults(clientBuilder =>
        {
            clientBuilder.AddStandardResilienceHandler();
        });

        app = await appHost.BuildAsync();
        await app.StartAsync();

        ApiClient = InitializeClient(appHost);
    }

    /// <inheritdoc/>
    public async Task DisposeAsync()
    {
        await app.StopAsync();
        if (app is IAsyncDisposable asyncDisposable)
        {
            await asyncDisposable.DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            app.Dispose();
        }
    }

    private ApiClient InitializeClient(IDistributedApplicationTestingBuilder appHost)
    {
        var apiResource = appHost.Resources
            .FirstOrDefault(resource => resource.Name == Constants.ApiResourceName);
        apiResource.TryGetAnnotationsOfType<EndpointAnnotation>(out var annotations);

        var apiEndpointAnnotation = annotations.First();
        var apiBaseUrl = $"https://{apiEndpointAnnotation.TargetHost}:{apiEndpointAnnotation.Port}";

        return new ApiClient(apiBaseUrl, app.CreateHttpClient(Constants.ApiResourceName));
    }
}
