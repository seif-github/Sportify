using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using sportify.BLL.DTOs;
using sportify.BLL.Services.Contracts;
using sportify.DAL.Data;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;

namespace sportify.BLL.Services
{
    public class LeagueTeamCountUpdateService: ILeagueTeamCountUpdateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LeagueTeamCountUpdateService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task UpdateTeamCountAsync(LeagueTeamCountUpdateDTO model)
        {
            var league = await _unitOfWork.LeagueRepository.GetByIdAsync(model.LeagueID);
            if (league != null)
            {
                league.NumberOfTeams = model.TeamCount;
                _unitOfWork.LeagueRepository.Update(league);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}
