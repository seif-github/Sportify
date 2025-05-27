using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sportify.BLL.DTOs;

namespace sportify.BLL.Services.Contracts
{
    public interface ILeagueService
    {
        Task<List<LeagueDTO>> GetAllAsync();
        Task<LeagueDTO?> GetByIdAsync(int id);
        Task<string?> GetOrganizerIdByLeagueId(int leagueId);
        Task<List<LeagueDTO?>?> GetOrganizerLeaguesById(string organizerId);
        Task AddAsync(LeagueDTO model);
        Task<LeagueDTO> AddAndReturnAsync(LeagueDTO model);
        Task UpdateAsync(LeagueDTO model);
        Task DeleteAsync(int id);
        Task<LeagueReportDTO?> GetLeagueReportDataAsync(int leagueId);
        void ClearTracking();
    }
}
