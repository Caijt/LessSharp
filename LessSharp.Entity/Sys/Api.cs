using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Entity.Sys
{
    public class Api : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsCommon { get; set; }
        public ApiPermissionType PermissionType { get; set; }
        public List<MenuApi> MenuApis { get; set; }
    }
    public enum ApiPermissionType
    {
        READ,
        WRITE,
        REVIEW
    }
}
