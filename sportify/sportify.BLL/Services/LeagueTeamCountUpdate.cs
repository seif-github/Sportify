using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class LeagueTeamCountUpdateService: ILeagueTeamCountUpdateService
    {
        private readonly IGenericRepository<League> _genericRepo;
        private readonly IMapper _mapper;

        public LeagueTeamCountUpdateService(IGenericRepository<League> genericRepo, IMapper mapper)
        {
            this._genericRepo = genericRepo;
            this._mapper = mapper;
        }

        public async Task UpdateTeamCountAsync(LeagueTeamCountUpdateDTO model)
        {
            var league = await _genericRepo.GetByIdAsync(model.LeagueID);
            if (league != null)
            {
                league.NumberOfTeams = model.TeamCount;
                await _genericRepo.UpdateAsync(league);
                await _genericRepo.SaveChangesAsync();
            }
        }
    }
}
