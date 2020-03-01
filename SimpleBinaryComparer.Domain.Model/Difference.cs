using System.Collections.Generic;

namespace SimpleBinaryComparer.Domain.Model
{
    /// <summary>
    /// Difference object that contains the lenght of the diff and the offsets
    /// </summary>
    public class Difference
    {
        /// <summary>
        /// The offsets of the diff
        /// </summary>
        public IEnumerable<DifferenceOffset> OffSets { get; set; }

        /// <summary>
        /// The lenght of the diff
        /// </summary>
        public int Length { get; set; }
    }
}