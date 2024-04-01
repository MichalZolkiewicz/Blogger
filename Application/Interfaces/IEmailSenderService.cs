using Domain.Enums;

namespace Application.Interfaces;

public interface IEmailSenderService
{
    Task<bool> Send(string to, string subject, EmailTemplate template, object model);
}
