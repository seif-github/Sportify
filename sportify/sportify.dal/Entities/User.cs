using System;
using System.Collections.Generic;

namespace sportify.DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? LastLogin { get; set; }

    public int? FavoriteTeamId { get; set; }

    public virtual Team? FavoriteTeam { get; set; }

    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}
