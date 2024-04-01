using Application.Interfaces;
using Domain.Enums;
using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Application.Services.Emails;

public class EmailSenderService : IEmailSenderService
{
    private const string TemplatePath = "Application.Services.Emails.Templates.{0}.cshtml";
    private readonly IFluentEmail _fluentEmail;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(IFluentEmail fluentEmail, ILogger<EmailSenderService> logger)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
    }
    public async Task<bool> Send(string to, string subject, EmailTemplate template, object model)
    {
        var result = await _fluentEmail.To(to).Subject(subject)
            .UsingTemplateFromEmbedded(string.Format(TemplatePath, template), ToExpando(model), GetType().Assembly)
            .SendAsync();

        if(!result.Successful)
        {
            _logger.LogError("Failed to send an email!\n{Errors}", string.Join(Environment.NewLine, result.ErrorMessages));
        }
        return result.Successful;
    }

    #region Helper methods

    private static ExpandoObject ToExpando(object model)
    {
        if (model is ExpandoObject exp)
        {
            return exp;
        }

        IDictionary<string, object> expando = new ExpandoObject();
        foreach(var propertyDescrioptor in model.GetType().GetTypeInfo().GetProperties())
        {
            var obj = propertyDescrioptor.GetValue(model);

            if(obj != null && IsAnonymousType(obj.GetType()))
            {
                obj = ToExpando(obj);
            }
            expando.Add(propertyDescrioptor.Name, obj);
        }
        return (ExpandoObject)expando;
    }

    private static bool IsAnonymousType(Type type)
    {
        bool hasComplierGeneratedAttribute = type.GetTypeInfo()
            .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
            .Any();

        bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
        bool isAnonymousType = hasComplierGeneratedAttribute && nameContainsAnonymousType;

        return isAnonymousType;
    }
 
    #endregion
}