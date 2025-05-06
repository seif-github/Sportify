using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class MatchStatistic
{
    public int Id { get; set; }

    public int MatchId { get; set; }

    public int TeamId { get; set; }

    public int Goals { get; set; }

    public int YellowCards { get; set; }

    public int RedCards { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Team Team { get; set; } = null!;
}
