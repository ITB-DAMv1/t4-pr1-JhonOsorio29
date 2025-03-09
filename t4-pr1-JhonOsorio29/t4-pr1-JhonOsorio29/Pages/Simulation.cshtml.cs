using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public static List<Simulation> Simulations { get; set; } = new List<Simulation>();

        [BindProperty]
        public Simulation NewSimulation { get; set; }

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

            NewSimulation.CostKWh ??= 20;
            NewSimulation.PriceKWh ??= 20;

            NewSimulation.calculateEnergy();

            Simulations.Add(NewSimulation);

            SaveSimulationsToCsv();

            return RedirectToPage();
        }

        private void SaveSimulationsToCsv()
        {
            // Verificamos si la carpeta Model-data existe, si no, la crea
            string directory = Path.GetDirectoryName(CsvFilePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Verificamos si el archivo existe para agregar encabezados solo la primera vez
            bool fileExists = System.IO.File.Exists(CsvFilePath);

            using (var writer = new StreamWriter(CsvFilePath, append: true, Encoding.UTF8))
            {
                if (!fileExists)
                {
                    writer.WriteLine("Fecha;Tipo;Parametro;Rati;EnergiaGenerada;CostoKWh;PrecioKWh;TotalCostoKWh;TotalPrecioKWh");
                }

                var sim = Simulations[^1]; // Tomamos solo la última simulación agregada
                writer.WriteLine($"{sim.Date:yyyy-MM-dd};{sim.Tipe};{sim.Parameter};{sim.Rati};{sim.EnergyGenerated};{sim.CostKWh};{sim.PriceKWh};{sim.TotalCostKWh};{sim.TotalPriceKWh}");
            }
        }

        private List<Simulation> LoadSimulationsFromCsv()
        {
            List<Simulation> simulations = new List<Simulation>();

            if (!System.IO.File.Exists(CsvFilePath))
                return simulations;

            var lines = System.IO.File.ReadAllLines(CsvFilePath, Encoding.UTF8).Skip(1); // Nos saltamos la cabecera
            foreach (var line in lines)
            {
                var data = line.Split(';'); // Leemos los datos separados por ;
                if (data.Length == 9)
                {
                    simulations.Add(new Simulation
                    {
                        Date = DateTime.ParseExact(data[0], "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Tipe = data[1],
                        Parameter = double.Parse(data[2], CultureInfo.InvariantCulture),
                        Rati = double.Parse(data[3], CultureInfo.InvariantCulture),
                        EnergyGenerated = double.Parse(data[4], CultureInfo.InvariantCulture),
                        CostKWh = double.Parse(data[5], CultureInfo.InvariantCulture),
                        PriceKWh = double.Parse(data[6], CultureInfo.InvariantCulture)
                    });
                }
            }
            return simulations;
        }
    }
}
