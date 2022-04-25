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
        try
        {
            var models = await _urlUseCases.GetAllUrlsAsync();
            return models.Adapt<IEnumerable<URLDTO>>();
        }
        catch (Exception)
        {
            throw;
        }
    }
}