using System.Collections.Generic;

namespace SSD_Status.WPF.ViewModels.Sources
{
    internal class AggregationTypeViewModelSource
    {
        public static IEnumerable<EnumerableViewModel<AggregationType>> GetAggregationTypes()
        {
            yield return new EnumerableViewModel<AggregationType>(AggregationType.Day, "Day");
            yield return new EnumerableViewModel<AggregationType>(AggregationType.Week, "Week");
            yield return new EnumerableViewModel<AggregationType>(AggregationType.Fortnight, "Fortnight");
            yield return new EnumerableViewModel<AggregationType>(AggregationType.Month, "Month");
        }
    }
}
