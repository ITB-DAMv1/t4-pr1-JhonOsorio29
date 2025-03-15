using System.Reflection.Metadata;

namespace t4_pr1_JhonOsorio29.Model
{
    public class Eolic : Simulation
    {
        public Eolic()
        {
            Type = "eolic";
        }
        public override void CalculateEnergy()
        {
            EnergyGenerated = Parameter * Rati;
        }
    }
}
