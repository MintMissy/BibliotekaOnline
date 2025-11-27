using TEST.Models;

namespace TEST.Services
{
    public class KsiazkiWedlugOddzialuIGatunkuStrategy : IReportStrategy<Dictionary<Oddzial, Dictionary<Gatunek, int>>>
    {
        public Dictionary<Oddzial, Dictionary<Gatunek, int>> GenerateReport(List<Kopie> kopie)
        {
            return kopie
                .Where(k => k.Ksiazka.Gatunek != null)
                .GroupBy(k => new { k.Oddzial, k.Ksiazka.Gatunek })
                .Select(g => new { g.Key.Oddzial, g.Key.Gatunek, Count = g.Count() })
                .ToList()
                .GroupBy(x => x.Oddzial!)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(x => x.Gatunek!, x => x.Count)
                );
        }
    }
}

