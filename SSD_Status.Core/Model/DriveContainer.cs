using System.Collections.Generic;

namespace SSD_Status.Core.Model
{
    static class DriveContainer
    {
        static IReadOnlyList<SsdDrive> Drives
        {
            get
            {
                var drives = new List<SsdDrive>();
                return drives;
            }
        }
    }
}
