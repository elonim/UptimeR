
using UptimeR.ML.Domain.Models.RavenModels;

namespace Uptimer.ML.Reader;

public interface IReader
{
    Task<ServiceAnomalies> GetAnomaliesForDate(DateOnly date);
    Task<RavenLog> GetAnomaliesForService(DateOnly date, String serviceName);
}
