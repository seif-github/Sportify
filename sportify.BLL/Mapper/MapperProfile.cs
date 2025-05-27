using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.DTOs;
using sportify.DAL.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<ApplicationUser, ProfileDTO>().ReverseMap();
        CreateMap<ApplicationUser, RegisterUserDTO>().ReverseMap();
        CreateMap<ApplicationUser, LoginUserDTO>().ReverseMap();
        CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        CreateMap<League, LeagueDTO>().ReverseMap();
        CreateMap<Team, TeamDTO>().ReverseMap();
        CreateMap<Match, MatchDTO>().ReverseMap();

        CreateMap<MatchDTO, Match>()
            .ForMember(dest => dest.FirstTeam, opt => opt.Ignore())
            .ForMember(dest => dest.SecondTeam, opt => opt.Ignore())
            .ForMember(dest => dest.League, opt => opt.Ignore());
        CreateMap<Match, MatchDTO>()
            .ForMember(dest => dest.FirstTeamName, opt => opt.MapFrom(src => src.FirstTeam.Name))
            .ForMember(dest => dest.SecondTeamName, opt => opt.MapFrom(src => src.SecondTeam.Name))
            .ForMember(dest => dest.Result, opt => opt.MapFrom(src => src.Result));
    }
}

