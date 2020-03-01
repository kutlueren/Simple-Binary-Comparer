using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBinaryComparer.Domain.Model
{
    /// <summary>
    /// Comparison entity. Contains left, right array and a unique key
    /// </summary>
    public class Comparison
    {
        /// <summary>
        /// Unique key for entity
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Left array of the data
        /// </summary>
        public byte[] LeftArray { get; set; }

        /// <summary>
        /// Right array of the data
        /// </summary>
        public byte[] RightArray { get; set; }

        /// <summary>
        /// Checks whether two arrays are same size
        /// </summary>
        /// <returns>True if sizes are the same</returns>
        public bool EqualSize()
        {
            return LeftArray.Length == RightArray.Length;
        }

        /// <summary>
        /// Checks whether left and right array sequentially equal
        /// </summary>
        /// <returns>True if the arrays are sequantially equal</returns>
        public bool IsEqual()
        {
            return LeftArray.SequenceEqual(RightArray);
        }

        /// <summary>
        /// Finds the diffs in the left array
        /// </summary>
        /// <returns>Difference containing the length and offsets of the diff</returns>
        public Difference FindDiffsInLeft()
        {
            byte[] diffs = LeftArray.Except(RightArray).ToArray();

            return GetDifference(diffs, LeftArray);
        }

        /// <summary>
        /// Finds the diffs in the right array
        /// </summary>
        /// <returns>Difference containing the length and offsets of the diff</returns>
        public Difference FindDiffsInRight()
        {
            byte[] diffs = RightArray.Except(LeftArray).ToArray();

            return GetDifference(diffs, RightArray);
        }

        /// <summary>
        /// Finds the offsets of the diffs
        /// </summary>
        /// <param name="diffs">diffs of the arrays</param>
        /// <param name="arrayToFindIndex">array to find the offsetss of the detected diffs</param>
        /// <returns>Difference containing the length and offsets of the diff</returns>
        private Difference GetDifference(byte[] diffs, byte[] arrayToFindIndex)
        {
            List<DifferenceOffset> offsetList = new List<DifferenceOffset>();

            offsetList = diffs.Select(t => new DifferenceOffset() { OffSet = Array.FindIndex(arrayToFindIndex, row => row == t) }).ToList();

            Difference difference = new Difference();

            difference.OffSets = offsetList;
            difference.Length = diffs.Length;

            return difference;
        }
    }
}