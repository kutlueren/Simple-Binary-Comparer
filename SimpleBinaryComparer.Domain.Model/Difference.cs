using System;
using System.Collections.Generic;
using System.Text;

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
