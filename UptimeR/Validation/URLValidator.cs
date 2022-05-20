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
        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))//Indpygget check for url ok
            return true;
        if (ValidIPAddress(url))//manuel test hvis ikke url men ip
            return true;
        return false;
    }

    private bool ValidIPAddress(string url)
    {
        string[] parts = url.Split(':');

        if (parts.Length < 1 || parts.Length > 2)//test for at url ikke er blefet delt i for mange dele
            return false;

        if (parts.Length == 2 && !parts[1].All(char.IsDigit))//test for at port er et tal
            return false;

        var port = int.Parse(parts[1]);
        if (port <= 0 || port > 65535)//test for at port er mellem 0 og 65535
            return false;

        if (IPAddress.TryParse(parts[0], out _))//test for at ip er valid
            return true;

        return false;
    }
}