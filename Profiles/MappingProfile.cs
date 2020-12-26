using AutoMapper;
using KhatmaBackEnd.Entities;
using KhatmaBackEnd.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KhatmaBackEnd.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Group, GroupViewModel>()
                .ForMember(dest => dest.admin, opt => opt.MapFrom(src => src.Users.Where(c => c.Role ==Utilites.Enums.Roles.GroupAdmin).FirstOrDefault()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
            CreateMap<GroupViewModel, Group>();
            CreateMap<UserForAdd, User>();
            CreateMap<User, UserForAdd>();
            CreateMap<GroupForCreateViewModel, Group>();
            CreateMap<Group, UserGroup>();
            CreateMap<User, UserData>();
            CreateMap<UserData, User>();
        }
    }
}
