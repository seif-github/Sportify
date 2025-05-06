using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class Match
{
    public int Id { get; set; }

    public int HomeTeamId { get; set; }

    public int AwayTeamId { get; set; }

    public int TournamentId { get; set; }

    public DateTime MatchDate { get; set; }

    public virtual Team AwayTeam { get; set; } = null!;

    public virtual Team HomeTeam { get; set; } = null!;

    public virtual ICollection<MatchStatistic> MatchStatistics { get; set; } = new List<MatchStatistic>();

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();

    public virtual ICollection<TeamTournamentMatchStatistic> TeamTournamentMatchStatistics { get; set; } = new List<TeamTournamentMatchStatistic>();

    public virtual Tournament Tournament { get; set; } = null!;
}
