using UptimeR.Application.Commands.URLRequests;

namespace UptimeR.Application.Interfaces;

public interface IURLUseCases
{
    Task<List<ReadAllURLSSettings>> GetAllUrlsAsync();
    List<ReadAllURLSSettings> GetAllUrls();
    Task AddURL(CreateURLRequest command);
    void UpdateURL(UpdateURLRequest command);
    void DeleteURL(DeleteURLRequest command);
}
