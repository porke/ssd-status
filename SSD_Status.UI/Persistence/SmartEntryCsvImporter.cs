﻿using SSD_Status.Core.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SSD_Status.WPF.Persistence
{
    internal class SmartEntryCsvImporter : ISmartEntryCsvImporter
    {
        public IList<SmartDataEntry> ImportSmartEntries(string filename)
        {
            var entries = new List<SmartDataEntry>();
            foreach (var line in File.ReadAllLines(filename).Skip(1))
            {
                var fileEntries = line.Split(';');
                DateTime timestamp = DateTime.ParseExact(fileEntries[0], "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                int powerOnHours = int.Parse(fileEntries[1]);
                int wearLevelling = int.Parse(fileEntries[2]);
                double writtenGb = double.Parse(fileEntries[3], CultureInfo.InvariantCulture);

                entries.Add(new SmartDataEntry(timestamp, writtenGb, powerOnHours, 0, wearLevelling));
            }

            return entries;
        }
    }
}
