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

        // Enhanced Match mapping for dashboard
        CreateMap<Match, MatchDTO>()
            .ForMember(dest => dest.MatchDate, opt => opt.MapFrom(src => src.Date))
            .ForMember(dest => dest.HomeTeamName, opt => opt.MapFrom(src => src.FirstTeam.Name))
            .ForMember(dest => dest.AwayTeamName, opt => opt.MapFrom(src => src.SecondTeam.Name))
            .ForMember(dest => dest.HomeTeamScore, opt => opt.MapFrom(src => src.FirstTeamGoals))
            .ForMember(dest => dest.AwayTeamScore, opt => opt.MapFrom(src => src.SecondTeamGoals))
            .ForMember(dest => dest.LeagueName, opt => opt.MapFrom(src => src.League.Name));
    }
}

