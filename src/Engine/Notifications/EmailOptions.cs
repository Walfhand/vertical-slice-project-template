namespace Engine.Notifications;

public class EmailOptions
{
    public string SenderAddress { get; set; } = null!;
    public string? SenderDisplayName { get; set; }
    public string Host { get; set; } = null!;
    public int Port { get; set; } = 587;
    public bool EnableSsl { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }
}