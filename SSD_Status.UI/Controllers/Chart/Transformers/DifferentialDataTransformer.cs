using System;
using System.Linq;
using System.Collections.Generic;

namespace SSD_Status.WPF.Controllers.Chart.Transformers
{
    internal class DifferentialDataTransformer : IChartDataTransformer
    {
        public IEnumerable<KeyValuePair<DateTime, double>> Transform(IEnumerable<KeyValuePair<DateTime, double>> data)
        {           
            var differences = data.Zip(data.Skip(1), (x, y) => new KeyValuePair<DateTime, double>(y.Key, y.Value - x.Value));            
            return new List<KeyValuePair<DateTime, double>>
                    { new KeyValuePair<DateTime, double>(data.First().Key, 0)}
                    .Concat(differences);
        }
    }
}
