using TEST.Models;

namespace TEST.Services
{
    public class KsiazkiWedlugOddzialuIRokuStrategy : IReportStrategy<Dictionary<Oddzial, Dictionary<int, int>>>
    {
        public Dictionary<Oddzial, Dictionary<int, int>> GenerateReport(List<Kopie> kopie)
        {
            return kopie
                .GroupBy(k => new { k.Oddzial, k.Ksiazka.RokWydania })
                .Select(g => new { g.Key.Oddzial, g.Key.RokWydania, Count = g.Count() })
                .ToList()
                .GroupBy(x => x.Oddzial!)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToDictionary(x => x.RokWydania, x => x.Count)
                );
        }
    }
}

