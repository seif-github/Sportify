using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class Player
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int TeamId { get; set; }

    public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new List<PlayerStatistic>();

    public virtual Team Team { get; set; } = null!;
}
