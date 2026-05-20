using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HLL_SPA_calculator.Models
{
    public class ArtilleryPoint
    {
        [JsonPropertyName("distance")]
        public double Distance { get; set; }

        [JsonPropertyName("mil")]
        public double Mil { get; set; }
    }
}
