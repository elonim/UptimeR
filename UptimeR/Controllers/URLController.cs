using Mapster;
using Microsoft.AspNetCore.Mvc;
using UptimeR.Application.Interfaces;
using UptimeR.DTOs;

namespace UptimeR.Controllers;

public class URLController
{
    private readonly IURLUseCases _urlUseCases;
    public URLController(IURLUseCases urlUseCases)
    {
        _urlUseCases = urlUseCases;
    }

    [HttpGet]
    [Route("/api/")]
    public async Task<IEnumerable<URLDTO>> GetUrls()
    {
        var models = await _urlUseCases.GetAllUrlsAsync();
        var urls = models.Adapt<IEnumerable<URLDTO>>();
        return  urls;
    }
}