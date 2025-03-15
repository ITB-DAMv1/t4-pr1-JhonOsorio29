using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using t4_pr1_JhonOsorio29.Model;

namespace t4_pr1_JhonOsorio29.Pages
{
    public class WaterConsumModel : PageModel
    {
        private static string CsvFilePath = @"Model-data/consum_aigua_cat_per_comarques.csv";

        [BindProperty]
        public WaterConsumption NewWaterConsumption { get; set; } = new();
        public List<WaterConsumption> TopMunicipalities { get; set; } = [];
        public List<ComarcaConsumption> AvgComarcaConsumption { get; set; } = [];
        public List<string> SuspiciousConsumptions { get; set; } = [];
        public List<string> IncreasingTrendMunicipalities { get; set; } = [];
        public List<WaterConsumption> WaterConsumptions { get; set; } = [];

        public void OnGet()
        {
            var data = LoadWaterConsumptionData();
            WaterConsumptions = LoadWaterConsumptionsFromXml();

            if (data.Any())
            {
                // Filtra por años
                int latestYear = data.Max(x => x.Year);
                    
                
                // 1 consumo ultimos 10 años 
                TopMunicipalities = data
                    .Where(x => x.Year == latestYear)
                    .OrderByDescending(x => x.TotalConsumption)
                    .Take(10)
                    .ToList();

                // 2 consumo por comarcas
                AvgComarcaConsumption = [.. data
                    .GroupBy(x => x.Comarca)
                    .Select(g => new ComarcaConsumption
                    {
                        Comarca = g.Key,
                        AvgConsumption = g.Average(x => x.TotalConsumption)
                    })
                    .OrderByDescending(x => x.AvgConsumption)];

                // 3 valores sospechosos
                SuspiciousConsumptions = data
                    .Where(x => x.TotalConsumption > 999999)
                    .Select(x => $"{x.Comarca} ({x.Year}) - {x.TotalConsumption} m³")
                    .ToList();


                // 4 tendencias 5 años
                IncreasingTrendMunicipalities = data
                .GroupBy(x => x.Comarca)
                .Where(g =>
                {
                    var lastFiveYears = g.OrderByDescending(x => x.Year).Take(5).OrderBy(x => x.Year).ToList();
                    bool isIncreasing = true;
                    for (int i = 1; i < lastFiveYears.Count; i++)
                    {
                        if (lastFiveYears[i].TotalConsumption <= lastFiveYears[i - 1].TotalConsumption)
                        {
                            isIncreasing = false;
                            break;
                        }
                    }
                    if (isIncreasing)
                    {
                        Console.WriteLine($"Comarca con tendencia creciente: {g.Key}");
                    }
                    return isIncreasing;
                })
                .Select(g => g.Key!)
                .ToList();
            }
        }
        private List<WaterConsumption> LoadWaterConsumptionsFromXml()
        {
            var xmlFilePath = @"Model-data/water_consumption_data.xml";

            if (!System.IO.File.Exists(xmlFilePath))
            {
                return new List<WaterConsumption>();
            }

            var xmlSerializer = new XmlSerializer(typeof(List<WaterConsumption>));
            using (var reader = new StreamReader(xmlFilePath))
            {
                return (List<WaterConsumption>)xmlSerializer.Deserialize(reader);
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Guardar el nuevo consumo en un archivo XML
            SaveWaterConsumptionToXml(NewWaterConsumption);

            // Limpiar el formulario después de guardar
            NewWaterConsumption = new();
            WaterConsumptions = LoadWaterConsumptionsFromXml();

            return Page();

        }

        private void SaveWaterConsumptionToXml(WaterConsumption waterConsumption)
        {
            var xmlFilePath = @"Model-data/water_consumption_data.xml";

            // Crear el archivo XML si no existe
            if (!System.IO.File.Exists(xmlFilePath))
            {
                var initialXmlSerializer = new XmlSerializer(typeof(List<WaterConsumption>));
                using (var writer = new StreamWriter(xmlFilePath))
                {
                    initialXmlSerializer.Serialize(writer, new List<WaterConsumption>());
                }
            }

            // Leer los datos existentes
            var xmlSerializer = new XmlSerializer(typeof(List<WaterConsumption>));
            List<WaterConsumption> data;
            using (var reader = new StreamReader(xmlFilePath))
            {
                data = (List<WaterConsumption>)xmlSerializer.Deserialize(reader);
            }

            // Agregar el nuevo consumo
            data.Add(waterConsumption);

            // Guardar los datos actualizados
            using (var writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, data);
            }
            data.Add(waterConsumption);

            // Guardar los datos actualizados
            using (var writer = new StreamWriter(xmlFilePath))
            {
                xmlSerializer.Serialize(writer, data);
            }
        }


       
        private List<WaterConsumption> LoadWaterConsumptionData()
        {
            List<WaterConsumption> data = [];

            if (!System.IO.File.Exists(CsvFilePath))
                return data;

            var lines = System.IO.File.ReadAllLines(CsvFilePath, Encoding.UTF8);
            for (int i = 1; i < lines.Length; i++) // Saltar la cabecera
            {
                var values = lines[i].Split(',');

                try
                {
                    data.Add(new WaterConsumption
                    {
                        Year = int.TryParse(values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var year) ? year : 0,
                        ComarcaCode = values[1],
                        Comarca = values[2],
                        Population = int.TryParse(values[3], NumberStyles.Any, CultureInfo.InvariantCulture, out var population) ? population : 0,
                        DomesticConsumption = double.TryParse(values[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var domestic) ? domestic : 0,
                        EconomicConsumption = double.TryParse(values[5], NumberStyles.Any, CultureInfo.InvariantCulture, out var economic) ? economic : 0,
                        TotalConsumption = double.TryParse(values[6], NumberStyles.Any, CultureInfo.InvariantCulture, out var total) ? total : 0,
                        PerCapitaConsumption = double.TryParse(values[7], NumberStyles.Any, CultureInfo.InvariantCulture, out var perCapita) ? perCapita : 0
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error en línea {i + 1}: {ex.Message}");
                }
            }
            return data;
        }
    }

}
