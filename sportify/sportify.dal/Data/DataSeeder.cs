using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Sportify.DAL.Entities;

namespace Sportify.DAL.Data
{
    public static class DataSeeder
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return; 
            }

            var users = new User[]
            {
                new User { Username = "admin", Email = "admin@example.com", PasswordHash = "hashed_password_here", Role = "Admin" },
                new User { Username = "manager", Email = "manager@example.com", PasswordHash = "hashed_password_here", Role = "Manager" },
                new User { Username = "user", Email = "user@example.com", PasswordHash = "hashed_password_here", Role = "User" }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var teams = new Team[]
            {
                new Team { Name = "الأهلي", LogoUrl = "/images/teams/ahly.png", Location = "القاهرة", FoundedYear = "1907", StadiumName = "استاد السلام" },
                new Team { Name = "الزمالك", LogoUrl = "/images/teams/zamalek.png", Location = "القاهرة", FoundedYear = "1911", StadiumName = "استاد القاهرة" },
                new Team { Name = "بيراميدز", LogoUrl = "/images/teams/pyramids.png", Location = "القاهرة", FoundedYear = "2008", StadiumName = "استاد الدفاع الجوي" }
            };

            context.Teams.AddRange(teams);
            context.SaveChanges();

            var tournaments = new Tournament[]
            {
                new Tournament { 
                    Name = "الدوري المصري الممتاز", 
                    Description = "الدوري المصري الممتاز لموسم 2024-2025", 
                    StartDate = new DateTime(2024, 8, 1), 
                    EndDate = new DateTime(2025, 5, 31),
                    Status = TournamentStatus.Active
                },
                new Tournament { 
                    Name = "كأس مصر", 
                    Description = "كأس مصر لموسم 2024-2025", 
                    StartDate = new DateTime(2024, 9, 15), 
                    EndDate = new DateTime(2025, 6, 30),
                    Status = TournamentStatus.Upcoming
                }
            };

            context.Tournaments.AddRange(tournaments);
            context.SaveChanges();

            var teamTournaments = new TeamTournament[]
            {
                new TeamTournament { TeamId = 1, TournamentId = 1 },
                new TeamTournament { TeamId = 2, TournamentId = 1 },
                new TeamTournament { TeamId = 3, TournamentId = 1 },
                new TeamTournament { TeamId = 1, TournamentId = 2 },
                new TeamTournament { TeamId = 2, TournamentId = 2 }
            };

            context.TeamTournaments.AddRange(teamTournaments);
            context.SaveChanges();

            var players = new Player[]
            {
                new Player { Name = "محمد الشناوي", TeamId = 1 },
                new Player { Name = "محمد أبو تريكة", TeamId = 1 },
                new Player { Name = "محمود جنش", TeamId = 2 },
                new Player { Name = "حازم إمام", TeamId = 2 },
                new Player { Name = "إبراهيم عادل", TeamId = 3 }
            };

            context.Players.AddRange(players);
            context.SaveChanges();

            var matches = new Match[]
            {
                new Match { 
                    HomeTeamId = 1, 
                    AwayTeamId = 2, 
                    TournamentId = 1, 
                    MatchDate = DateTime.Now.AddDays(7)
                },
                new Match { 
                    HomeTeamId = 2, 
                    AwayTeamId = 3, 
                    TournamentId = 1, 
                    MatchDate = DateTime.Now.AddDays(14)
                }
            };

            context.Matches.AddRange(matches);
            context.SaveChanges();
        }
    }
}