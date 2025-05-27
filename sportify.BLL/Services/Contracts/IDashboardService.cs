using sportify.BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.Services.Contracts
{
    public interface IDashboardService
    {
        Task<DashboardDTO> GetDashboardDataAsync(string userId);
    }
}
