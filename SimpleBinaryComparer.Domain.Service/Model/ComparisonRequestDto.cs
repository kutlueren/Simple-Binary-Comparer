namespace SimpleBinaryComparer.Domain.Service.Model
{
    /// <summary>
    /// Comparison dto. Used to indicate which array to compare
    /// </summary>
    public class ComparisonRequestDto
    {
        /// <summary>
        /// Unique index of array which will be compared
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Type of the comparison that indicates the array to compare to the other. Ex: if type left is selected then the left array will be compare to right and diffs in the left array would be found
        /// </summary>
        public ComparisonEnum ValueType { get; set; }
    }
}
