using System;

namespace t4_pr1_JhonOsorio29.Model
{
    public interface ISimulation
    {
        DateTime Date { get; set; }
        double Parameter { get; set; }
        double Rati { get; set; }
        double? CostKWh { get; set; }
        double? PriceKWh { get; set; }
        double EnergyGenerated { get; set; }
        double? TotalCostKWh { get; }
        double? TotalPriceKWh { get; }

        void CalculateEnergy();
    }
}
