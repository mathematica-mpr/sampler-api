using System.Collections.Generic;
using Newtonsoft.Json;

namespace sampler_api.Models
{
    public class Simulate
    {

        [JsonProperty("Cases")]
        public IList<Coordinate> Cases { get; set; }

        [JsonProperty("NonCases")]
        public IList<Coordinate> NonCases { get; set; }

        [JsonProperty("Prevalence")]
        public IList<Coordinate> Prevalence { get; set; }

        [JsonProperty("TruePos")]
        public IList<Coordinate> TruePos { get; set; }

        [JsonProperty("FalNeg")]
        public IList<Coordinate> FalNeg { get; set; }

        [JsonProperty("Positives")]
        public IList<Coordinate> Positives { get; set; }

        [JsonProperty("TrueNeg")]
        public IList<Coordinate> TrueNeg { get; set; }

        [JsonProperty("FalPos")]
        public IList<Coordinate> FalPos { get; set; }

        [JsonProperty("Negatives")]
        public IList<Coordinate> Negatives { get; set; }

        [JsonProperty("PPV")]
        public IList<Coordinate> PPV { get; set; }

        [JsonProperty("NPV")]
        public IList<Coordinate> NPV { get; set; }

        [JsonProperty("Sens")]
        public IList<Coordinate> Sens { get; set; }

        [JsonProperty("Spec")]
        public IList<Coordinate> Spec { get; set; }


        [JsonProperty("Fpr")]
        public IList<Coordinate> Fpr { get; set; }

        [JsonProperty("Fnr")]
        public IList<Coordinate> Fnr { get; set; }

        [JsonProperty("Fdr")]
        public IList<Coordinate> Fdr { get; set; }

        [JsonProperty("For")]
        public IList<Coordinate> For { get; set; }
    }

    public class Coordinate
    {
        [JsonProperty("X")]
        public float X { get; set; }

        [JsonProperty("Y")]
        public float Y { get; set; }

        [JsonProperty("C")]
        public float C { get; set; }
    }
}