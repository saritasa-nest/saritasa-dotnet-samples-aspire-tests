namespace AspireTesting.Models;

public record SmtpSettings
{
    required public string Host { get; init; }
    required public int Port { get; init; }
    public bool UseSsl { get; init; }
    public string UserName { get; init; }
    public string Password { get; init; }
    required public string FromAddress { get; init; }
}
