using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.ApiService.Qywx.Dto
{
    public class UserInfo : ApiResult
    {
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public string OpenId { get; set; }
    }
}
