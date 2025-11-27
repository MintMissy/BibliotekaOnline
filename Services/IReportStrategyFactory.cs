using TEST.Models;

namespace TEST.Services
{
    public interface IReportStrategyFactory
    {
        IReportStrategy<Dictionary<Oddzial, Dictionary<int, int>>> GetOddzialRokStrategy();
        IReportStrategy<Dictionary<Oddzial, Dictionary<Gatunek, int>>> GetOddzialGatunekStrategy();
        IReportStrategy<Dictionary<Oddzial, int>> GetSumaStronStrategy();
    }
}

