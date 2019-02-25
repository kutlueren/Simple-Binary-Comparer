using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleBinaryComparer.Domain.Model
{
    public class Comparison
    {
        public int Id { get; set; }

        public byte[] LeftArray { get; set; }

        public byte[] RightArray { get; set; }

        public bool EqualSize()
        {
            return LeftArray.Length == RightArray.Length;
        }

        public bool IsEqual()
        {
            return LeftArray.SequenceEqual(RightArray);
        }

        public Difference FindDiffsInLeft()
        {
            byte[] diffs = LeftArray.Except(RightArray).ToArray();

            return GetDifference(diffs, LeftArray);
        }

        public Difference FindDiffsInRight()
        {
            byte[] diffs = RightArray.Except(LeftArray).ToArray();

            return GetDifference(diffs, RightArray);
        }

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
