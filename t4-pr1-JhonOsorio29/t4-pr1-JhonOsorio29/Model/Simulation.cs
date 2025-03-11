using System.ComponentModel.DataAnnotations;
using System;

namespace t4_pr1_JhonOsorio29.Model
{
    public abstract class Simulation : ISimulation
    {
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string? Type { get; protected set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "el valor debe de ser mayor de 0")]
        public double Parameter { get; set; } // = Horas de sol, velocidad del viento, cabal de agua

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "el valor debe de ser de 0 a 3")]
        public double Rati { get; set; }

        private double? _costKwh;
        private double? _priceKwh;

        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe de ser mayor a 0")]
        public double? CostKWh
        {
            get => _costKwh ?? 20;
            set => _costKwh = value;
        }

        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe de ser mayor a 0")]
        public double? PriceKWh
        {
            get => _priceKwh ?? 20;
            set => _priceKwh = value;
        }

        public double EnergyGenerated { get; set;}
        public double? TotalCostKWh => EnergyGenerated * CostKWh;
        public double? TotalPriceKWh => EnergyGenerated * PriceKWh;

        public override string ToString()
        {
            return $"{Date:yyyy-MM-dd};{Type};{Parameter};{Rati};{EnergyGenerated};{CostKWh};{PriceKWh};{TotalCostKWh};{TotalPriceKWh}";
        }

        

        public abstract void CalculateEnergy();

        
    }
}