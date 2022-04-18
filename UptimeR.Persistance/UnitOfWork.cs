using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using UptimeR.Application.Interfaces;

namespace UptimeR.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private IDbContextTransaction _transaction;
    private readonly DatabaseContext _context;

    public UnitOfWork(DatabaseContext context)
    {
        _context = context;
    }

    public void stoptracking()
    {
        _context.ChangeTracker.Clear();
    }

    public bool SaveChanges()
    {
        try
        {
            return _context.SaveChanges() >= 0;
        }
        catch(Exception)
        {
            throw new Exception("Error on saving changes");
        }
    }

    public async Task<bool> SaveChangesAsync()
    {
        try
        {
            return await _context.SaveChangesAsync() >= 0;
        }
        catch(Exception)
        {
            throw new Exception("Error on saving changes");
        }
    }

    public void Beginerializable()
    {
        _transaction = _context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
    }

    public void CommitSerializable()
    {
        try
        {
            _transaction.Commit();
            SaveChanges();
        }
        catch(Exception)
        {
            _transaction.Rollback();
            throw new Exception("Error on saving changes");
        }
    }
}