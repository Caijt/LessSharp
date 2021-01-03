using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.ApiService.Qywx.Dto
{
    public class AccessToken : ApiResult
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
    }
}
