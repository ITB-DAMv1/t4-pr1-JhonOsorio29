using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using t4_pr1_JhonOsorio29.Model;

namespace t4_pr1_JhonOsorio29.Pages
{
    public class SimulationModel : PageModel
    {
        private static string CsvFilePath = @"Model-data\simulation-data.csv";

        public static List<Simulation> Simulations { get; set; } = [];

        [BindProperty]
        public string? SelectedTipe { get; set; } // Tipo de simulación seleccionada

        [BindProperty]
        public double Parameter { get; set; }

        [BindProperty]
        public double Rati { get; set; }

        [BindProperty]
        public double? CostKWh { get; set; }

        [BindProperty]
        public double? PriceKWh { get; set; }

        public void OnGet()
        {
            Simulations = LoadSimulationsFromCsv();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            CostKWh ??= 20;
            PriceKWh ??= 20;

            Simulation newSimulation = CreateSimulation(SelectedTipe, Parameter, Rati, CostKWh, PriceKWh);

            if (newSimulation == null)
            {
                ModelState.AddModelError(string.Empty, "Tipo de simulación no válido.");
                return Page();
            }

            newSimulation.CalculateEnergy();
            Simulations.Add(newSimulation);
            SaveSimulationToCsv(newSimulation);

            return RedirectToPage();
        }

        private Simulation CreateSimulation(string tipe,
                                            double parameter,
                                            double rati,
                                            double? costKWh,
                                            double? priceKWh)
        {
            return tipe.ToLower() switch
            {
                "solar" => new Solar { Parameter = parameter, Rati = rati, CostKWh = costKWh, PriceKWh = priceKWh },
                "eolic" => new Eolic { Parameter = parameter, Rati = rati, CostKWh = costKWh, PriceKWh = priceKWh },
                "hidroelectric" => new Hidroelectric { Parameter = parameter, Rati = rati, CostKWh = costKWh, PriceKWh = priceKWh },
                _ => throw new ArgumentException($"tipo de simulacion no valida")
            };
        }

        private void SaveSimulationToCsv(Simulation sim)
        {

            string directory = Path.GetDirectoryName(CsvFilePath);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            bool fileExists = System.IO.File.Exists(CsvFilePath);

            using var writer = new StreamWriter(CsvFilePath, append: true, Encoding.UTF8);
            if (!fileExists)
            {
                writer.WriteLine("Fecha;Tipo;Parametro;Rati;EnergiaGenerada;CostoKWh;PrecioKWh;TotalCostoKWh;TotalPrecioKWh");
            }

            writer.WriteLine(sim.ToString());
        }

        private List<Simulation> LoadSimulationsFromCsv()
        {
            List<Simulation> simulations = [];

            if (!System.IO.File.Exists(CsvFilePath))
                return simulations;

            var lines = System.IO.File.ReadAllLines(CsvFilePath, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++) // Saltamos la cabecera
            {
                var data = lines[i].Split(';');
                if (data.Length == 9)
                {
                    Simulation simulation = CreateSimulation(data[1],
                        double.Parse(data[2], CultureInfo.InvariantCulture),
                        double.Parse(data[3], CultureInfo.InvariantCulture),
                        double.Parse(data[5], CultureInfo.InvariantCulture),
                        double.Parse(data[6], CultureInfo.InvariantCulture));

                    if (simulation != null)
                    {
                        simulation.Date = DateTime.ParseExact(data[0], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        simulation.EnergyGenerated = double.Parse(data[4], CultureInfo.InvariantCulture);
                        simulations.Add(simulation);

                    }
                }
            }
            return simulations;
        }
    }
}
