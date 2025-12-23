namespace Engine.Notifications;

public record EmailMessage(string Recipient, string Subject, string Body, bool IsHtml = true, string? ReplyTo = null);
