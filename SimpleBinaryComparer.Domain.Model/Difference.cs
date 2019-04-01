using System.Collections.Generic;

namespace SimpleBinaryComparer.Domain.Model
{
    public class Difference
    {
        public IEnumerable<DifferenceOffset> OffSets { get; set; }

        public int Length { get; set; }
    }

    public class DifferenceOffset
    {
        public int OffSet { get; set; }
    }
}
