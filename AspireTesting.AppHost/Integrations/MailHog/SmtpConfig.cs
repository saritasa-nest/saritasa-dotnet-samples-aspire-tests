namespace AspireTesting.AppHost.Integrations.MailHog;

public class SmtpConfig
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool UseSsl { get; set; }
    public string FromAddress { get; set; } = string.Empty;
}
