using System.ComponentModel.DataAnnotations;
using System;
namespace t4_pr1_JhonOsorio29.Model
{
    public class Simulation
    {
        public DateTime Date { get; set; } = DateTime.Now;

        [Required]
        public string Tipe { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "el valor debe de ser mayor de 0")]
        public double Parameter { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "el valor debe de ser de 0 a 3")]
        public double Rati { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo debe de ser mayor a 0")]
        public double CostKWh { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe de ser mayor a 0")]
        public double PriceKWh { get; set; }

        public double EnergyGenerated { get; set;}
        public double TotalCostKWh => EnergyGenerated * CostKWh;
        public double TotalPriceKWh => EnergyGenerated * PriceKWh;

        public void calcularEnergia()
        {
            switch (Tipo)
        }

    }
}