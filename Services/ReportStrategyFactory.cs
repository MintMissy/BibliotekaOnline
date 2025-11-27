namespace TEST.Services
{
    public class ReportStrategyFactory : IReportStrategyFactory
    {
        private readonly KsiazkiWedlugOddzialuIRokuStrategy _oddzialRokStrategy;
        private readonly KsiazkiWedlugOddzialuIGatunkuStrategy _oddzialGatunekStrategy;
        private readonly SumaStronWedlugOddzialuStrategy _sumaStronStrategy;

        public ReportStrategyFactory(
            KsiazkiWedlugOddzialuIRokuStrategy oddzialRokStrategy,
            KsiazkiWedlugOddzialuIGatunkuStrategy oddzialGatunekStrategy,
            SumaStronWedlugOddzialuStrategy sumaStronStrategy)
        {
            _oddzialRokStrategy = oddzialRokStrategy;
            _oddzialGatunekStrategy = oddzialGatunekStrategy;
            _sumaStronStrategy = sumaStronStrategy;
        }

        public IReportStrategy<Dictionary<Models.Oddzial, Dictionary<int, int>>> GetOddzialRokStrategy() => _oddzialRokStrategy;

        public IReportStrategy<Dictionary<Models.Oddzial, Dictionary<Models.Gatunek, int>>> GetOddzialGatunekStrategy() => _oddzialGatunekStrategy;

        public IReportStrategy<Dictionary<Models.Oddzial, int>> GetSumaStronStrategy() => _sumaStronStrategy;
    }
}

