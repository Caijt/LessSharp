using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Option
{
    public class JwtOption
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string AccessSecretKey { get; set; }
        public int AccessExpiresIn { get; set; }
        public int RefreshExpiresIn { get; set; }
        public int RememberRefreshExpiresIn { get; set; }
        public string RefreshSecretKey { get; set; }
    }
}
