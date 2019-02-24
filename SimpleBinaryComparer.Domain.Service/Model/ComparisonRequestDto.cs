using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBinaryComparer.Domain.Service.Model
{
    public class ComparisonInsertRequestDto
    {
        public int Id { get; set; }

        public byte[] Value { get; set; }

        public ComparisonEnum ValueType { get; set; }
    }

    public class ComparisonRequestDto
    {
        public int Id { get; set; }

        public ComparisonEnum ValueType { get; set; }
    }

    public enum ComparisonEnum
    {
        Left = 1,
        Right = 2
    }
}
