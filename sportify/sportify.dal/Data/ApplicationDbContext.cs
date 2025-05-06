using Microsoft.EntityFrameworkCore;
using Sportify.DAL.Entities;

namespace Sportify.DAL.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchStatistic> MatchStatistics { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<TeamTournament> TeamTournaments { get; set; }
        public DbSet<TeamTournamentMatchStatistic> TeamTournamentMatchStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.FavoriteTeam)
                .WithMany(t => t.Fans)
                .HasForeignKey(u => u.FavoriteTeamId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            modelBuilder.Entity<Team>()
                .HasOne(t => t.ManagerUser)
                .WithMany()
                .HasForeignKey(t => t.ManagerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany(t => t.Players)
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<TeamTournament>()
                .HasKey(tt => new { tt.TeamId, tt.TournamentId });

            modelBuilder.Entity<TeamTournament>()
                .HasOne(tt => tt.Team)
                .WithMany(t => t.TeamTournaments)
                .HasForeignKey(tt => tt.TeamId);

            modelBuilder.Entity<TeamTournament>()
                .HasOne(tt => tt.Tournament)
                .WithMany(t => t.TeamTournaments)
                .HasForeignKey(tt => tt.TournamentId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TournamentId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchStatistic>()
                .HasOne(ms => ms.Match)
                .WithMany(m => m.MatchStatistics)
                .HasForeignKey(ms => ms.MatchId);

            modelBuilder.Entity<MatchStatistic>()
                .HasOne(ms => ms.Team)
                .WithMany()
                .HasForeignKey(ms => ms.TeamId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasOne(ps => ps.Match)
                .WithMany(m => m.PlayerStatistics)
                .HasForeignKey(ps => ps.MatchId);

            modelBuilder.Entity<TeamTournamentMatchStatistic>()
                .HasOne(ttms => ttms.Team)
                .WithMany()
                .HasForeignKey(ttms => ttms.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamTournamentMatchStatistic>()
                .HasOne(ttms => ttms.Tournament)
                .WithMany()
                .HasForeignKey(ttms => ttms.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamTournamentMatchStatistic>()
                .HasOne(ttms => ttms.Match)
                .WithMany()
                .HasForeignKey(ttms => ttms.MatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}