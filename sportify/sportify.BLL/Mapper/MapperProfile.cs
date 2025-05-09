using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.DTOs;
using sportify.DAL.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ApplicationUser, RegisterUserDTO>().ReverseMap();
        CreateMap<ApplicationUser, LoginUserDTO>().ReverseMap();
        CreateMap<League, LeagueDTO>().ReverseMap();
        CreateMap<Team, TeamDTO>().ReverseMap();
        CreateMap<Match, MatchDTO>().ReverseMap();
    }
}
