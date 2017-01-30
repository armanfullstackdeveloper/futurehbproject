using System;

namespace BusinessLogic.Helpers
{
    public class MyExceptionHandler : ApplicationException
    {
        public string Input { get; set; }
        public MyExceptionHandler() : base() { }
        public MyExceptionHandler(string message) : base(message) { }
        public MyExceptionHandler(string message, Exception innerException) : base(message, innerException) { }

        public MyExceptionHandler(string message, Exception innerException, string input)
            : base(message, innerException)
        {
            Input = input.Replace("\r\n", string.Empty).Replace("\"",string.Empty);
        }
    }
}
