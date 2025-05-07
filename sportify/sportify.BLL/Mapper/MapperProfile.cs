using AutoMapper;
using sportify.BLL.DTOs;
using sportify.DAL.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Tournament, TournamentDTO>().ReverseMap();
        CreateMap<Team, TeamDTO>().ReverseMap();
    }
}
