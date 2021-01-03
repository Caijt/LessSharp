using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LessSharp.Dto
{
    public class AttachUploadDto
    {
        [Required]
        public IFormFile File { get; set; }
        [Required]
        public Guid? EntityGuid { get; set; }
        public string Type { get; set; }

        public bool IsPublic { get; set; }
    }
}
