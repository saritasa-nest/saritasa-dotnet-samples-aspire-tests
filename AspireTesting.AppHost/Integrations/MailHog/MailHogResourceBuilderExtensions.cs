namespace AspireTesting.AppHost.Integrations.MailHog;

public static class MailHogResourceBuilderExtensions
{
    internal const int DefaultHttpPort = 8025;
    internal const int DefaultSmtpPort = 1025;

    internal const string SmtpEndpointName = "smtp";
    internal const string HttpEndpointName = "http";

    internal const string DefaultAddressFrom = "smtp@example.com";

    public static IResourceBuilder<MailHogResource> AddMailHog(
        this IDistributedApplicationBuilder builder,
        string name)
    {
        var resource = new MailHogResource(name);

        var mailhog = builder.AddResource(resource)
            .WithImage("mailhog/mailhog");

        return mailhog
            .WithPorts()
            .FromAddress(DefaultAddressFrom);
    }

    public static IResourceBuilder<MailHogResource> WithPorts(
        this IResourceBuilder<MailHogResource> builder,
        int httpPort = DefaultHttpPort,
        int smtpPort = DefaultSmtpPort)
    {
        builder.Resource.TryGetAnnotationsOfType<EndpointAnnotation>(out var endpoints);

        if (endpoints != null && endpoints.Any())
        {
            foreach (var endpoint in endpoints)
            {
                builder.Resource.Annotations.Remove(endpoint);
            }
        }

        builder
            .WithEndpoint(targetPort: DefaultHttpPort, 
                port: httpPort, 
                scheme: HttpEndpointName,
                name: HttpEndpointName)
            .WithEndpoint(targetPort: DefaultSmtpPort, 
                port: smtpPort, 
                scheme: SmtpEndpointName,
                name: SmtpEndpointName);

        return builder;
    }

    public static IResourceBuilder<MailHogResource> FromAddress(
        this IResourceBuilder<MailHogResource> builder,
        string address)
    {
        builder.Resource.SmtpConfig.FromAddress = address;
        return builder;
    }

    public static IResourceBuilder<MailHogResource> UseSsl(
        this IResourceBuilder<MailHogResource> builder)
    {
        builder.Resource.SmtpConfig.UseSsl = true;
        return builder;
    }

    public static IResourceBuilder<TDestination> WithReference<TDestination>(
        this IResourceBuilder<TDestination> builder, IResourceBuilder<MailHogResource> source)
        where TDestination : IResourceWithEnvironment
    {
        var endpoint = source.GetEndpoint(SmtpEndpointName);

        // Enter the context to access endpoint.
        builder.WithEnvironment(context =>
        {
            source.Resource.SmtpConfig.Host = endpoint.Host;
            source.Resource.SmtpConfig.Port = endpoint.Port;
    
            var smtpConfigType = source.Resource.SmtpConfig.GetType();

            foreach (var property in smtpConfigType.GetProperties())
            {
                var value = property.GetValue(source.Resource.SmtpConfig);
            
                var key = $"{source.Resource.Name}__{property.Name}";
            
                context.EnvironmentVariables.Add(key, value?.ToString() ?? string.Empty);
            }
        });

        return builder;
    }
}
