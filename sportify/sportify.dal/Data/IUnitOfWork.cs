using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ILeagueRepository LeagueRepository { get; }
        ITeamRepository TeamRepository { get; }
        IMatchRepository MatchRepository { get; }
        IUserRepository UserRepository { get; }
        IDashboardRepository DashboardRepository { get; }

        Task<int> CompleteAsync();
        void Rollback();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        void ClearTracking();
    }
}
