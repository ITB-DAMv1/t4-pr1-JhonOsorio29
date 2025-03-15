namespace t4_pr1_JhonOsorio29.Interfaces
{
    public interface IWaterConsum
    {
        int Year { get; set; }
        string ComarcaCode { get; set; }
        string Comarca { get; set; }
        int Population { get; set; }
        double DomesticConsumption { get; set; }
        double EconomicConsumption { get; set; }
        double TotalConsumption { get; set; }
        double PerCapitaConsumption { get; set; }
    }
}
