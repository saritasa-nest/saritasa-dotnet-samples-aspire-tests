namespace AspireTesting.AppHost.Integrations.MailHog;

public class MailHogResource : ContainerResource
{
    internal SmtpConfig SmtpConfig { get; }

    public MailHogResource(string name) : base(name)
    {
        SmtpConfig = new();
    }
}
