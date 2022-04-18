namespace UptimeR.Application.Interfaces;

public interface IUnitOfWork
{
    bool SaveChanges();
    Task<bool> SaveChangesAsync();
    void Beginerializable();
    void CommitSerializable();
    void stoptracking();
}
