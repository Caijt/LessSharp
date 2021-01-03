using AutoMapper;
using System.Linq;
using LessSharp.Dto.Sys;
using LessSharp.Entity.Sys;

namespace LessSharp.MapperConfiguration.Sys
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleSaveDto, Role>().AfterMap((src, dest) =>
            {
                dest.RoleMenus.ForEach(r =>
                {
                    r.RoleId = dest.Id;
                });
            });
        }
    }
}
