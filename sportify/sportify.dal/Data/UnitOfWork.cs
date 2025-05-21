using Microsoft.EntityFrameworkCore;
using sportify.DAL.Entities;
using sportify.DAL.Repositories.Contracts;
using sportify.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.DAL.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SportifyContext _context;
        private bool _disposed;

        private ILeagueRepository _leagueRepository;
        private ITeamRepository _teamRepository;
        private IMatchRepository _matchRepository;
        private IUserRepository _userRepository;
        private IDashboardRepository _dashboardRepository;

        public UnitOfWork(SportifyContext context)
        {
            _context = context;
        }

        public ILeagueRepository LeagueRepository =>
            _leagueRepository ??= new LeagueRepository(_context);

        public ITeamRepository TeamRepository =>
            _teamRepository ??= new TeamRepository(_context);

        public IMatchRepository MatchRepository =>
            _matchRepository ??= new MatchRepository(_context);

        public IUserRepository UserRepository =>
            _userRepository ??= new UserRepository(_context);

        public IDashboardRepository DashboardRepository =>
            _dashboardRepository ??= new DashboardRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            foreach (var entry in _context.ChangeTracker.Entries())
            {
                entry.State = EntityState.Detached;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public void ClearTracking()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
