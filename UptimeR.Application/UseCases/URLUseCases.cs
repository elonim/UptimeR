using Mapster;
using UptimeR.Application.Commands.URLRequests;
using UptimeR.Application.Interfaces;
using UptimeR.Domain;

namespace UptimeR.Application.UseCases;

public class URLUseCases : IURLUseCases
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IURLRepository _urlRepository;

    public URLUseCases(IUnitOfWork unitOfWork, IURLRepository urlRepository)
    {
        _unitOfWork = unitOfWork;
        _urlRepository = urlRepository;
    }

    async Task<List<ReadAllURLSSettings>> IURLUseCases.GetAllUrlsAsync()
    {
        var domainUrls = await _urlRepository.GetAllAsync();
        _unitOfWork.stoptracking();
        var urls = domainUrls.Adapt<List<ReadAllURLSSettings>>();
        return urls;
    }
    List<ReadAllURLSSettings> IURLUseCases.GetAllUrls()
    {
        var domainUrls = _urlRepository.GetAll();
        _unitOfWork.stoptracking(); //For some reason tracking fucks up in IHostedService
        var urls = domainUrls.Adapt<List<ReadAllURLSSettings>>();
        return urls;
    }

    async Task IURLUseCases.AddURL(CreateURLRequest command)
    {
        var url = command.Adapt<URL>();
        await _urlRepository.AddAsync(url);
        _unitOfWork.SaveChanges();
    }

    void IURLUseCases.UpdateURL(UpdateURLRequest command)
    {
        var url = command.Adapt<URL>();
        _unitOfWork.Beginerializable();
        _urlRepository.Update(url);
        _unitOfWork.CommitSerializable();
    }

    void IURLUseCases.DeleteURL(DeleteURLRequest command)
    {
        var url = command.Adapt<URL>();
        _unitOfWork.Beginerializable();
        _urlRepository.Remove(url);
        _unitOfWork.CommitSerializable();
    }
}