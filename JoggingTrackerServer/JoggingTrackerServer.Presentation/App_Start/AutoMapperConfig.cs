using AutoMapper;
using JoggingTrackerServer.Domain.Entities;
using JoggingTrackerServer.Domain.Enums;
using JoggingTrackerServer.Presentation.Models;
using System;

namespace JoggingTrackerServer.Presentation.App_Start
{
    public static class AutoMapperConfig
    {
        public static void Configure()
        {
            ConfigureJog();
        }

        private static void ConfigureJog()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Jog, JogViewModel>()
                .ForMember(dto => dto.UserName, e => e.MapFrom(x => x.User.UserName));
                cfg.CreateMap<JogViewModel, Jog>();
                cfg.CreateMap<User, UserViewModel>();
                cfg.CreateMap<UserViewModel, User>()
                .ForMember(vm => vm.TokenExpiration, e => e.MapFrom(x => DateTime.Now));
                cfg.CreateMap<User, UserInfoViewModel>()
                .ForMember(dto => dto.Permission, e => e.MapFrom(x => Enum.GetName(typeof(UserPermission), x.Permission)));
            });
        }
    }
}