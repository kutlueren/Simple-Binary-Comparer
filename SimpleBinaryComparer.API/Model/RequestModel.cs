using SimpleBinaryComparer.Domain.Service.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.API.Model
{
    public class RequestModel
    {
        public byte[] Value { get; set; }
    }

    public class CompareRequestModel
    {
        public ComparisonEnum Type { get; set; }
    }
}
