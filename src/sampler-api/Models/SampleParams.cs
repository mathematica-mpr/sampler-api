namespace sampler_api.Models
{
    public class SimulateParams : Unique
    {
        public string Population { get; set; }
        public string Prev { get; set; }
        public string Tp { get; set; }
        public string Fp { get; set; }
        public string Fn { get; set; }
        public string Tn { get; set; }
    }
}