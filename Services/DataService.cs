using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.Json;
using HLL_SPA_calculator.Models;

namespace HLL_SPA_calculator.Services
{
    public class DataService
    {
        public ArtilleryType LoadArtillery(string path)
        {
            string json =
                File.ReadAllText(path);

            return JsonSerializer.Deserialize<ArtilleryType>(json);
        }
    }
}
