using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;
using System.Globalization;
using FileWorking = System.IO;
using t4_pr1_JhonOsorio29.Model;

namespace t4_pr1_JhonOsorio29.Pages
{
    public class SimulationModel : PageModel
    {
		public string FileErrorMessage;
		public List<SimulationModel> Products { get; set; } = new List<SimulationModel> { };
		public void OnGet()
		{
			string filePath = @"ModelData\product.txt";
			if (FileWorking.File.Exists(filePath))
			{
				string[] lines = FileWorking.File.ReadAllLines(filePath);
				foreach (string line in lines)
				{

					string[] parts = line.Split('|');
					if (parts.Length == 4)
					{
						Simulation product = new Simulation();
						Simulation.Id = int.Parse(parts[0]);
						Simulation.Name = parts[1];
						product.Amount = int.Parse(parts[2]);
						product.Price = decimal.Parse(parts[3], CultureInfo.InvariantCulture);
						Products.Add(product);
					}
					else
					{
						FileErrorMessage = "Error de carrega dels atributs d'un producte";
					}
				}
			}
			else
			{
				FileErrorMessage = "Error de carrega de dades";
			}
			Debug.WriteLine(Path.GetFullPath(filePath));
		}
	}

}
