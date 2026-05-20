using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace HLL_SPA_calculator.Models
{
    public class ArtilleryType
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("table")]
        public List<ArtilleryPoint> Table { get; set; }
    }
}
