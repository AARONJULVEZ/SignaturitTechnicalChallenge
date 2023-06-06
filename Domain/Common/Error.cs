namespace Domain.Common
{
    public class Error
    {
        public int Code { get; }
        public string Message { get; }

        public Error(int code, string message)
        {
            if (code <= 0)
            {
                code = 500;
                message = "Response mapping error, code value must be superior to 0";
            }

            if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            {
                code = 500;
                message = "Response mapping error, message must have value";
            }

            Code = code;
            Message = message;
        }
    }
}
