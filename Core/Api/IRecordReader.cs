using System.Collections.Generic;

namespace SSD_Status.Core.Api
{
    public interface IRecordReader
    {    
        IReadOnlyList<Record> GetRecords();
    }
}
