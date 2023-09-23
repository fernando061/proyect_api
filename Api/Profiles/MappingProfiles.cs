using Api.Dtos;
using AutoMapper;
using Core.Entities;

namespace Api.Profiles;
public class MappingProfiles :Profile
{
    public MappingProfiles()
    {
        CreateMap<User, UserDto>()
           .ReverseMap();
           //.ForMember(origen => origen.RefreshTokens, dest => dest.Ignore())
           //.ForMember(origen => origen.Password, dest => dest.Ignore());
    }
}

