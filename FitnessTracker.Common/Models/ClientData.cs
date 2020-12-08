using Microsoft.Research.SEAL;

namespace FitnessTracker.Common.Models
{
    public class ClientData
    {
        public Ciphertext Prime { get; set; }

        public Ciphertext Factor1 { get; set; }

        public Ciphertext Factor2 { get; set; }
    }
}
