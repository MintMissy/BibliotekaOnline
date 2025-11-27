using TEST.Models;

namespace TEST.Services
{
    public interface IReportStrategy<TResult>
    {
        TResult GenerateReport(List<Kopie> kopie);
    }
}

