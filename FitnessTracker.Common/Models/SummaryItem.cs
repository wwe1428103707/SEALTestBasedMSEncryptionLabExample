namespace FitnessTracker.Common.Models
{
    public class AnswerItem
    {
        public string Factor1 { get; set; }

        public string Factor2 { get; set; }
    }

    public class EncAnswerItem
    {
        public string Prime { get; set; }

        public string Factor1 { get; set; }

        public string Factor2 { get; set; }

        public string PublicKey { get; set; }

        public string SecretKey { get; set; }
    }
}
