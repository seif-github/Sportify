using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class Tournament
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string LogoUrl { get; set; } = null!;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Match> Matches { get; set; } = new List<Match>();

    public virtual ICollection<TeamTournamentMatchStatistic> TeamTournamentMatchStatistics { get; set; } = new List<TeamTournamentMatchStatistic>();

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
