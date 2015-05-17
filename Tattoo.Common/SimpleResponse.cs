namespace Tattoo.Common
{
    public class SimpleResponse
    {
        public SimpleResponse()
        {
            Success = true;
            Message = string.Empty;
        }

        public bool Success { get; set; }

        public string Message { get; set; }
    }
}