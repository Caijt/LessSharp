using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using LessSharp.Dto;
using LessSharp.Entity;

namespace LessSharp.MapperConfiguration.Hr
{
    public class AttachProfile : Profile
    {
        public AttachProfile()
        {
            CreateMap<Attach, AttachDto>()
                .ForMember(dest => dest.PublicPath, opt => opt.MapFrom(src => src.IsPublic ? src.Path : ""));
        }
    }
}
