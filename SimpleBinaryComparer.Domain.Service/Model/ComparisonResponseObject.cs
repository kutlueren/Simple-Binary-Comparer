using SimpleBinaryComparer.Domain.Model;

namespace SimpleBinaryComparer.Domain.Service.Model
{
    public class ComparisonResponseObject
    {
        public bool Equal { get; set; }
        public bool SameSize { get; set; }
        public Difference Difference { get; set; }
    }
}