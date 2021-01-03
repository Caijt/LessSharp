using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.ApiService.Qywx.Dto
{
    public class User:ApiResult
    {
        public string Userid { get; set; }
        public string Name { get; set; }
        public int[] Department { get; set; }
        public int[] Order { get; set; }
        public string Mobile { get; set; }

    }
}
