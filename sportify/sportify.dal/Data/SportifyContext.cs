using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using sportify.DAL.Entities;

namespace sportify.DAL.Data;

public partial class SportifyContext : IdentityDbContext<ApplicationUser>

{
    public SportifyContext(DbContextOptions<SportifyContext> options)
        : base(options)
    {
    }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Match> Matches { get; set; }
   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<League>()
                .HasOne(l => l.Organizer)
                .WithMany(u => u.Leagues)
                .HasForeignKey(l => l.OrganizerID)
                .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.FirstTeam)
            .WithMany()
            .HasForeignKey(m => m.FirstTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Match>()
            .HasOne(m => m.SecondTeam)
            .WithMany()
            .HasForeignKey(m => m.SecondTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        //modelBuilder.Entity<Match>()
        //    .HasOne(m => m.Winner)
        //    .WithMany()
        //    .HasForeignKey(m => m.WinnerId)
        //    .OnDelete(DeleteBehavior.Restrict);

        OnModelCreatingPartial(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
