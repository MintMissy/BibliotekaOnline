namespace TEST.Models
{
    public class RaportyKsiazekViewModel
    {
        // Tab 1: Książki według Oddziału i Roku
        // Dictionary<Oddzial, Dictionary<Year, Count>>
        public Dictionary<Oddzial, Dictionary<int, int>>? KsiazkiWedlugOddzialuIRoku { get; set; }

        // Tab 2: Książki według Oddziału i Gatunku
        // Dictionary<Oddzial, Dictionary<Gatunek, Count>>
        public Dictionary<Oddzial, Dictionary<Gatunek, int>>? KsiazkiWedlugOddzialuIGatunku { get; set; }

        // Tab 3: Suma Stron według Oddziału
        // Dictionary<Oddzial, TotalSumOfPages>
        public Dictionary<Oddzial, int>? SumaStronWedlugOddzialu { get; set; }
    }
}

