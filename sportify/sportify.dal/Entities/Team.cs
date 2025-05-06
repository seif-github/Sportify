using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class Team
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string LogoUrl { get; set; } = null!;

    public string Location { get; set; } = null!;

    public string FoundedYear { get; set; } = null!;

    public string StadiumName { get; set; } = null!;

    public int? ManagerUserId { get; set; }

    public virtual User? ManagerUser { get; set; }

    public virtual ICollection<Match> MatchAwayTeams { get; set; } = new List<Match>();

    public virtual ICollection<Match> MatchHomeTeams { get; set; } = new List<Match>();

    public virtual ICollection<MatchStatistic> MatchStatistics { get; set; } = new List<MatchStatistic>();

    public virtual ICollection<Player> Players { get; set; } = new List<Player>();

    public virtual ICollection<TeamTournamentMatchStatistic> TeamTournamentMatchStatistics { get; set; } = new List<TeamTournamentMatchStatistic>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}
