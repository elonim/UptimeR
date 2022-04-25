using System.Net;
using FluentValidation;
using UptimeR.Application.Commands.URLRequests;

namespace UptimeR.Validation;

public class URLValidator : AbstractValidator<CreateURLRequest>
{
    public URLValidator()
    {
        RuleFor(x => x.Interval).Cascade(CascadeMode.Stop)
                                .GreaterThan(0)
                                .NotEmpty()
                                .WithMessage("Interval must be greater than 0");

        RuleFor(x => x.ServiceName).Cascade(CascadeMode.Stop)
                                   .NotEmpty()
                                   .Must(ValidServiceName)
                                   .WithMessage("Servicename is not valid");

        RuleFor(x => x.Url).Cascade(CascadeMode.Stop)
                           .NotEmpty()
                           .Must(ValidURL)
                           .WithMessage("URL is not valid");
    }

    private bool ValidServiceName(string servicename)
    {
        servicename = servicename.Replace(" ", "")
                                 .Replace("-", "")
                                 .Replace("_", "");
        return servicename.All(char.IsLetterOrDigit);
    }

    private bool ValidURL(string url)
    {
        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return true;

        if (ValidIPAddress(url))
            return true;

        return false;
    }

    private bool ValidIPAddress(string url)
    {
        string[] parts = url.Split(':');

        var port = true;
        if (parts.Length == 2 && !parts[1].All(char.IsDigit))
            port = false;

        if (IPAddress.TryParse(parts[0], out _) && port)
            return true;

        return false;
    }
}