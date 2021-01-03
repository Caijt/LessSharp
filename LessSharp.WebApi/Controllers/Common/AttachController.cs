using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LessSharp.Dto;
using LessSharp.Service;
using LessSharp.WebApi.ApiGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace LessSharp.WebApi.Controllers
{
    [OpenApiTag("附件管理")]
    [ApiGroup(ApiGroupNames.Common)]
    public class AttachController : AutoRouteControllerBase
    {
        private readonly AttachService _attachService;
        public AttachController(AttachService attachService)
        {
            _attachService = attachService;
        }

        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="attachUploadDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<AttachDto>> Upload([FromForm]AttachUploadDto attachUploadDto)
        {
            return await _attachService.UploadAsync(attachUploadDto);
        }

        /// <summary>
        /// 下载附件，无论什么类型都会弹出下载框
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Download(int id)
        {
            var result = _attachService.Download(id);
            return File(result.Stream, result.ContentType, result.FileName);
        }

        /// <summary>
        /// 加载附件，如果是网页能处理的类型，不会弹出下载框
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Load(int id)
        {
            var result = _attachService.Download(id);
            return File(result.Stream, result.ContentType);
        }

        /// <summary>
        /// 获取附件列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ApiResultDto<List<AttachDto>>> GetList([FromQuery]AttachQueryDto dto)
        {
            return await _attachService.GetListAsync(dto);
        }

        /// <summary>
        /// 根据Id删除附件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResultDto<int>> DeleteById([FromBody]int id) {
            return await _attachService.DeleteByIdAsync(id);
        }
    }
}