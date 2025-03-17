using AspireTesting.IntegrationTests.Abstractions;
using System.Text;

namespace AspireTesting.IntegrationTests.Implementations;

public partial class ApiClient : IApiClient
{
    private async Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
    }

    // Need to implement those two methods in addition to other 'PrepareRequestAsync' overload according to NSwag code generator.
    // Source: https://github.com/RicoSuter/NSwag/issues/3247#issue-785307049
    private Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
