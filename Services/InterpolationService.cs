using HLL_SPA_calculator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HLL_SPA_calculator.Services
{
    public class InterpolationService
    {
        public double InterpolateMil(
            List<ArtilleryPoint> table,
            double targetDistance)
        {
            for (int i = 0; i < table.Count - 1; i++)
            {
                var p1 = table[i];
                var p2 = table[i + 1];

                if (targetDistance >= p1.Distance &&
                    targetDistance <= p2.Distance)
                {
                    double ratio =
                        (targetDistance - p1.Distance) /
                        (p2.Distance - p1.Distance);

                    return p1.Mil +
                           ratio * (p2.Mil - p1.Mil);
                }
            }

            throw new Exception("Distance out of range.");
        }
    }
}
