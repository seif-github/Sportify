using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sportify.DAL.Entities;

namespace sportify.DAL.Data;

public partial class SportifyContext : DbContext
{
    public SportifyContext()
    {
    }

    public SportifyContext(DbContextOptions<SportifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Match> Matches { get; set; }

    public virtual DbSet<MatchStatistic> MatchStatistics { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerStatistic> PlayerStatistics { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<TeamTournamentMatchStatistic> TeamTournamentMatchStatistics { get; set; }

    public virtual DbSet<Tournament> Tournaments { get; set; }

    public virtual DbSet<User> Users { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Match>(entity =>
        {
            entity.HasIndex(e => e.AwayTeamId, "IX_Matches_AwayTeamId");

            entity.HasIndex(e => e.HomeTeamId, "IX_Matches_HomeTeamId");

            entity.HasIndex(e => e.TournamentId, "IX_Matches_TournamentId");

            entity.HasOne(d => d.AwayTeam).WithMany(p => p.MatchAwayTeams)
                .HasForeignKey(d => d.AwayTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.HomeTeam).WithMany(p => p.MatchHomeTeams)
                .HasForeignKey(d => d.HomeTeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Tournament).WithMany(p => p.Matches).HasForeignKey(d => d.TournamentId);
        });

        modelBuilder.Entity<MatchStatistic>(entity =>
        {
            entity.HasIndex(e => e.MatchId, "IX_MatchStatistics_MatchId");

            entity.HasIndex(e => e.TeamId, "IX_MatchStatistics_TeamId");

            entity.HasOne(d => d.Match).WithMany(p => p.MatchStatistics).HasForeignKey(d => d.MatchId);

            entity.HasOne(d => d.Team).WithMany(p => p.MatchStatistics).HasForeignKey(d => d.TeamId);
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasIndex(e => e.TeamId, "IX_Players_TeamId");

            entity.HasOne(d => d.Team).WithMany(p => p.Players).HasForeignKey(d => d.TeamId);
        });

        modelBuilder.Entity<PlayerStatistic>(entity =>
        {
            entity.HasIndex(e => e.MatchId, "IX_PlayerStatistics_MatchId");

            entity.HasIndex(e => e.PlayerId, "IX_PlayerStatistics_PlayerId");

            entity.HasOne(d => d.Match).WithMany(p => p.PlayerStatistics).HasForeignKey(d => d.MatchId);

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerStatistics).HasForeignKey(d => d.PlayerId);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasIndex(e => e.ManagerUserId, "IX_Teams_ManagerUserId");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.FoundedYear).HasMaxLength(50);
            entity.Property(e => e.Location).HasMaxLength(100);
            entity.Property(e => e.LogoUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StadiumName).HasMaxLength(255);

            entity.HasOne(d => d.ManagerUser).WithMany(p => p.Teams).HasForeignKey(d => d.ManagerUserId);

            entity.HasMany(d => d.Tournaments).WithMany(p => p.Teams)
                .UsingEntity<Dictionary<string, object>>(
                    "TeamTournament",
                    r => r.HasOne<Tournament>().WithMany().HasForeignKey("TournamentId"),
                    l => l.HasOne<Team>().WithMany().HasForeignKey("TeamId"),
                    j =>
                    {
                        j.HasKey("TeamId", "TournamentId");
                        j.ToTable("TeamTournaments");
                        j.HasIndex(new[] { "TournamentId" }, "IX_TeamTournaments_TournamentId");
                    });
        });

        modelBuilder.Entity<TeamTournamentMatchStatistic>(entity =>
        {
            entity.HasIndex(e => e.MatchId, "IX_TeamTournamentMatchStatistics_MatchId");

            entity.HasIndex(e => e.TeamId, "IX_TeamTournamentMatchStatistics_TeamId");

            entity.HasIndex(e => e.TournamentId, "IX_TeamTournamentMatchStatistics_TournamentId");

            entity.HasOne(d => d.Match).WithMany(p => p.TeamTournamentMatchStatistics)
                .HasForeignKey(d => d.MatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Team).WithMany(p => p.TeamTournamentMatchStatistics)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Tournament).WithMany(p => p.TeamTournamentMatchStatistics)
                .HasForeignKey(d => d.TournamentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Tournament>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.LogoUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.FavoriteTeamId, "IX_Users_FavoriteTeamId");

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.FavoriteTeam).WithMany(p => p.Users).HasForeignKey(d => d.FavoriteTeamId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
