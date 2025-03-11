using System.Reflection.Metadata;

namespace t4_pr1_JhonOsorio29.Model
{
    public class Hidroelectric : Simulation
    {
        public Hidroelectric()
        {
            Type = "hidroelectric";
        }
        public override void CalculateEnergy()
        {
            EnergyGenerated = Parameter * Rati;
        }
    }
}
