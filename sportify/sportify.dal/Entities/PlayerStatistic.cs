using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class PlayerStatistic
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int MatchId { get; set; }

    public int Goals { get; set; }

    public int Assists { get; set; }

    public int YellowCards { get; set; }

    public int RedCards { get; set; }

    public virtual Match Match { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
