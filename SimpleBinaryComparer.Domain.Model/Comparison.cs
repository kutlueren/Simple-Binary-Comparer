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

            List<DifferenceOffset> offsetList = new List<DifferenceOffset>();

            foreach (var diff in diffs)
            {
                offsetList.Add(new DifferenceOffset() { OffSet = Array.FindIndex(LeftArray, row => row == diff) });
            }

            Difference difference = new Difference();

            difference.OffSets = offsetList;
            difference.Length = diffs.Length;

            return difference;
        }

        public Difference FindDiffsInRight()
        {
            byte[] diffs = RightArray.Except(LeftArray).ToArray();

            List<DifferenceOffset> offsetList = new List<DifferenceOffset>();

            foreach (var diff in diffs)
            {
                offsetList.Add(new DifferenceOffset() { OffSet = Array.FindIndex(RightArray, row => row == diff) });
            }

            Difference difference = new Difference();

            difference.OffSets = offsetList;
            difference.Length = diffs.Length;

            return difference;
        }
    }
}
