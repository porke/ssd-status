using System;

namespace SSD_Status.WPF.ViewModels
{
    internal class EntryViewModel
    {    
        public int EntryId { get; set; }
        public DateTime Timestamp { get; set; }
        public double HostWrittenGb { get; set; }
        public int PowerOnHours { get; set; }
        public int WearLevelling { get; set; }
        public int PercentLifetimeLeft { get; set; }
    }
}
