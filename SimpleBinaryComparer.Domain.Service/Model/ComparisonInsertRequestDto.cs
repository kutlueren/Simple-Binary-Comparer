namespace SimpleBinaryComparer.Domain.Service.Model
{
    /// <summary>
    /// Comparison insert dto. Used to carry Id as int, Value as byte[], ValueType as ComparisonEnum
    /// </summary>
    public class ComparisonInsertRequestDto
    {
        /// <summary>
        /// Unique index to save array to persistency
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Input array to compare
        /// </summary>
        public byte[] Value { get; set; }

        /// <summary>
        /// Type of the array
        /// </summary>
        public ComparisonEnum ValueType { get; set; }
    }
}
