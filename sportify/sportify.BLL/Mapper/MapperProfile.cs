using AutoMapper;
using sportify.BLL.CustomModels;
using sportify.DAL.Entities;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Tournament, TournamentModel>().ReverseMap();
    }
}
