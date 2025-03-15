using t4_pr1_JhonOsorio29.Pages;

namespace t4_pr1_JhonOsorio29.Model
{
    public class Solar : Simulation
    {
        public Solar()
        {
            Type = "solar";
        }
        public override void CalculateEnergy()
        {
            EnergyGenerated = Parameter * Rati;
        }

    }
}
