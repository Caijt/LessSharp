using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class LoginResultDto
    {
        public string Status { get; set; }
        public string AccessToken { get; set; }
        public int AccessExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshExpiresIn { get; set; }

        public const string SUCCESS = "SUCCESS";
        public const string FAIL = "FAIL";
        public const string TOKEN_FAIL = "TOKEN_FAIL";
        public const string USER_FAIL = "USER_FAIL";
        public const string PASSWORD_FAIL = "PASSWORD_FAIL";
        public const string EMPLOYEE_FAIL = "EMPLOYEE_FAIL";
        public const string QYWX_USER_FAIL = "QYWX_USER_FAIL";
    }
}
