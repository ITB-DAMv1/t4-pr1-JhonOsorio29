using System;
using t4_pr1_JhonOsorio29.Interfaces;

namespace t4_pr1_JhonOsorio29.Model
{

    public  class WaterConsumption : IWaterConsum
    {
        public int Year { get; set; }  
        public string? ComarcaCode { get; set; }  
        public string? Comarca { get; set; } 
        public int Population { get; set; }  
        public double DomesticConsumption { get; set; }  
        public double EconomicConsumption { get; set; }  
        public double TotalConsumption { get; set; } 
        public double PerCapitaConsumption { get; set; }
        public double AvgConsumption { get; set; }


        public override string ToString()
        {
            return $"{Year};{ComarcaCode};{Comarca};{Population};{DomesticConsumption};{EconomicConsumption};{TotalConsumption};{PerCapitaConsumption}";
        }
    }
}
