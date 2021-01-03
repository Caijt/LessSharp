using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.ApiService.Qywx.Dto
{
    public class JsapiTicket : ApiResult
    {
        public string Ticket { get; set; }
        public int Expires_in { get; set; }
    }
}
