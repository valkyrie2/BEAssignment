using AutoMapper;
using BE.DataAccess;
using BE.Model;

namespace BE.Services.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserResponse>();
            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();
        }
    }
}