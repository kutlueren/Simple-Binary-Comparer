namespace SimpleBinaryComparer.Domain.Service.Model
{
    public class ResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }

        public static ResponseBase CreateError(string message)
        {
            return new ResponseBase() { Success = false, Message = message };
        }
    }
}
