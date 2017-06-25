using System.Collections.Generic;
using System.Management;

namespace SSD_Status.Core.Model
{
    public static class DriveContainer
    {
        public static IReadOnlyList<SsdDrive> Drives
        {
            get
            {
                var searcher = new ManagementObjectSearcher("Select * from Win32_DiskDrive");                
                ManagementObjectCollection driveObjects = searcher.Get();

                var outDrives = new List<SsdDrive>();
                foreach (var drive in driveObjects)
                {
                    ulong totalSectors = (ulong)drive["TotalSectors"];
                    uint bytesPerSector = (uint)drive["BytesPerSector"];
                    var driveObject = new SsdDrive()
                    {
                        Name = drive["Model"] as string,
                        SerialNo = (drive["SerialNumber"] as string).Trim(),
                        CapacityInGb = BytesToGigabytes(totalSectors * bytesPerSector)
                    };                    

                    outDrives.Add(driveObject);
                }

                return outDrives;
            }
        }

        private static double BytesToGigabytes(ulong bytes)
        {
            return bytes / 1024.0 / 1024.0 / 1024.0;
        }
    }
}
