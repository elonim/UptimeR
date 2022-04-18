using Microsoft.EntityFrameworkCore;
using UptimeR.Application.Interfaces;
using UptimeR.Domain;


namespace UptimeR.Persistance.Repositorys;

public class URLRepository :Repository<URL>, IURLRepository
{
    private readonly IDatabaseContext _context;
    public URLRepository(DatabaseContext context) : base(context)
    {
        _context = context;
    }
}