using System;
using System.Collections.Generic;
using System.Text;

namespace LessSharp.Dto
{
    public class AuthInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public List<PermissionMenuDto> PermissionMenus { get; set; }
        
    }
    public class PermissionMenuDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public bool CanMultipleOpen { get; set; }
        public bool IsPage { get; set; }
        public int? ParentId { get; set; }
        public bool CanRead { get; set; }
        public bool CanWrite { get; set; }
        public bool CanReview { get; set; }
    }
}
