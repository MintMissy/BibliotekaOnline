using Microsoft.EntityFrameworkCore;
using TEST.Context;
using TEST.Models;

namespace TEST.Services
{
    public class RaportyKsiazekService
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportStrategyFactory _strategyFactory;

        public RaportyKsiazekService(
            ApplicationDbContext context,
            IReportStrategyFactory strategyFactory)
        {
            _context = context;
            _strategyFactory = strategyFactory;
        }

        public RaportyKsiazekViewModel GenerateReports()
        {
            var viewModel = new RaportyKsiazekViewModel();
            var kopie = LoadKopieWithRelatedData();

            if (kopie == null || !kopie.Any())
            {
                return CreateEmptyViewModel();
            }

            var oddzialRokStrategy = _strategyFactory.GetOddzialRokStrategy();
            viewModel.KsiazkiWedlugOddzialuIRoku = oddzialRokStrategy.GenerateReport(kopie);

            var oddzialGatunekStrategy = _strategyFactory.GetOddzialGatunekStrategy();
            viewModel.KsiazkiWedlugOddzialuIGatunku = oddzialGatunekStrategy.GenerateReport(kopie);

            var sumaStronStrategy = _strategyFactory.GetSumaStronStrategy();
            viewModel.SumaStronWedlugOddzialu = sumaStronStrategy.GenerateReport(kopie);

            return viewModel;
        }

        private List<Kopie> LoadKopieWithRelatedData()
        {
            return _context.kopie?
                .Include(k => k.Oddzial)
                .Include(k => k.Ksiazka)
                .ThenInclude(k => k.Gatunek)
                .Where(k => k.Oddzial != null && k.Ksiazka != null)
                .ToList() ?? new List<Kopie>();
        }

        private static RaportyKsiazekViewModel CreateEmptyViewModel()
        {
            return new RaportyKsiazekViewModel
            {
                KsiazkiWedlugOddzialuIRoku = new Dictionary<Oddzial, Dictionary<int, int>>(),
                KsiazkiWedlugOddzialuIGatunku = new Dictionary<Oddzial, Dictionary<Gatunek, int>>(),
                SumaStronWedlugOddzialu = new Dictionary<Oddzial, int>()
            };
        }
    }
}

