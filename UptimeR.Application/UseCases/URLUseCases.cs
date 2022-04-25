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
        try
        {
            var domainUrls = await _urlRepository.GetAllAsync();
            _unitOfWork.stoptracking();
            var urls = domainUrls.Adapt<List<ReadAllURLSSettings>>();
            return urls;
        }
        catch (Exception)
        {
            throw new Exception("Error Getting URLs");
        }
    }
    List<ReadAllURLSSettings> IURLUseCases.GetAllUrls()
    {
        try
        {
            var domainUrls = _urlRepository.GetAll();
            _unitOfWork.stoptracking(); //For some reason tracking fucks up in IHostedService
            var urls = domainUrls.Adapt<List<ReadAllURLSSettings>>();
            return urls;
        }
        catch (Exception)
        {
            throw new Exception("Error Getting URLs");
        }
    }

    async Task IURLUseCases.AddURL(CreateURLRequest command)
    {
        try
        {
            var url = command.Adapt<URL>();
            await _urlRepository.AddAsync(url);
            _unitOfWork.SaveChanges();
        }
        catch (Exception)
        {
            throw new Exception("Error Adding URL");
        }
    }

    void IURLUseCases.UpdateURL(UpdateURLRequest command)
    {
        try
        {
            var url = command.Adapt<URL>();
            _unitOfWork.Beginerializable();
            _urlRepository.Update(url);
            _unitOfWork.CommitSerializable();
        }
        catch (Exception)
        {
            throw new Exception("Error Updating URL");
        }
    }

    void IURLUseCases.DeleteURL(DeleteURLRequest command)
    {
        try
        {
            var url = command.Adapt<URL>();
            _unitOfWork.Beginerializable();
            _urlRepository.Remove(url);
            _unitOfWork.CommitSerializable();
        }
        catch (Exception)
        {
            throw new Exception("Error Deleting URL");
        }
    }
}