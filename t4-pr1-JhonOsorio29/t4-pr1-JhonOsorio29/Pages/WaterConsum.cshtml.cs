using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using t4_pr1_JhonOsorio29.Model;

namespace t4_pr1_JhonOsorio29.Pages
{
    public class WaterConsumModel : PageModel
    {
        private static string CsvFilePath = @"Model-data\consum_aigua_cat_per_comarques.csv";


        public List<WaterConsum> ConsumAiguaList { get; set; } = new List<WaterConsum>();
        public List<WaterConsum> Top10Municipios { get; set; }
        public List<(string Comarca, double Mediana)> MedianaPorComarca { get; set; }
        public List<string> MunicipiosConsumoExcesivo { get; set; }
        public List<string> MunicipiosCrecientes { get; set; }

        public void OnGet()
        {
            ConsumAiguaList = LoadConsumAiguaFromCsv();

            if (ConsumAiguaList.Any())
            {
                int añoMasReciente = ConsumAiguaList.Max(c => c.Año);

                // Top 10 municipios con más consumo en el año más reciente
                var municipiosFiltrados = ConsumAiguaList
                    .Where(c => c.Año == añoMasReciente)
                    .OrderByDescending(c => c.Consumo)
                    .Take(10)
                    .ToList();

                if (municipiosFiltrados.Any())
                {
                    Top10Municipios = municipiosFiltrados;
                }

                // Cálculo de la mediana de agua por comarca
                MedianaPorComarca = ConsumAiguaList
                    .GroupBy(c => c.Comarca)
                    .Select(g => (g.Key, Mediana(g.Select(c => c.Consumo))))
                    .OrderByDescending(c => c.Item2)
                    .ToList();

                // Municipios con consumo superior a 999999
                MunicipiosConsumoExcesivo = ConsumAiguaList
                    .Where(c => c.Consumo > 999999)
                    .Select(c => c.Municipio)
                    .Distinct()
                    .ToList();

                // Municipios con tendencia creciente en los últimos 5 años
                MunicipiosCrecientes = ConsumAiguaList
                    .GroupBy(c => c.Municipio)
                    .Where(g =>
                        g.OrderByDescending(c => c.Año)
                         .Take(5)
                         .Select(c => c.Consumo)
                         .SequenceEqual(g.OrderByDescending(c => c.Año)
                                         .Take(5)
                                         .Select(c => c.Consumo)
                                         .OrderBy(c => c))) // Verifica si los valores están en orden ascendente
                    .Select(g => g.Key)
                    .ToList();
            }
        }

        private List<WaterConsum> LoadConsumAiguaFromCsv()
        {
            List<WaterConsum> lista = new List<WaterConsum>();

            if (!System.IO.File.Exists(CsvFilePath))
                return lista; // Devuelve una lista vacía si el archivo no existe

            var lines = System.IO.File.ReadAllLines(CsvFilePath);
            if (lines.Length <= 1) return lista; // Si solo tiene la cabecera, retorna vacío

            foreach (var line in lines.Skip(1)) // Saltamos la cabecera
            {
                var data = line.Split(';'); // Aseguramos que el delimitador sea ";"
                if (data.Length >= 4)
                {
                    lista.Add(new WaterConsum
                    {
                        Municipio = data[0],
                        Comarca = data[1],
                        Año = int.TryParse(data[2], out int year) ? year : 0,
                        Consumo = double.TryParse(data[3], NumberStyles.Float, CultureInfo.InvariantCulture, out double consumo) ? consumo : 0
                    });
                }
            }
            return lista;
        }
        private double Mediana(IEnumerable<double> valores)
        {
            var sorted = valores.OrderBy(v => v).ToList();
            int count = sorted.Count;
            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2.0;
            return sorted[count / 2];
        }

    }
}
