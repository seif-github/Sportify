using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class TeamTournamentMatchStatistic
{
    public int Id { get; set; }

    public int TeamId { get; set; }

    public int TournamentId { get; set; }

    public int MatchId { get; set; }

    public int GoalsScored { get; set; }

    public int GoalsConceded { get; set; }

    public int YellowCards { get; set; }

    public int RedCards { get; set; }

    public int Corners { get; set; }

    public int Fouls { get; set; }

    public int ShotsOnTarget { get; set; }

    public int ShotsOffTarget { get; set; }

    public int Possession { get; set; }

    public int Offsides { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;

    public virtual Tournament Tournament { get; set; } = null!;
}
