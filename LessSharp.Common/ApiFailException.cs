using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Common
{
    public class ApiFailException : Exception
    {
        public ApiFailCode Code { get; set; }
        private string _message;
        public override string Message => _message;
        public ApiFailException(ApiFailCode code)
        {
            Code = code;
        }
        public ApiFailException(ApiFailCode code, string message) : this(code)
        {
            _message = message;
        }
    }
}
