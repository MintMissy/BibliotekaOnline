using TEST.Models;

namespace TEST.Services
{
    public class SumaStronWedlugOddzialuStrategy : IReportStrategy<Dictionary<Oddzial, int>>
    {
        public Dictionary<Oddzial, int> GenerateReport(List<Kopie> kopie)
        {
            return kopie
                .GroupBy(k => k.Oddzial)
                .Select(g => new { Oddzial = g.Key, TotalPages = g.Sum(k => k.Ksiazka.LiczbaStron) })
                .ToList()
                .ToDictionary(x => x.Oddzial!, x => x.TotalPages);
        }
    }
}

