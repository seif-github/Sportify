using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace sportify.DAL.Entities
{
    public class Team
    {
        public int TeamID { get; set; } 
        public string Name { get; set; } = null!; 
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int TotalMatchesPlayed { get; set; }
        public int Points { get; set; }
        [ForeignKey("League")]
        public int LeagueID { get; set; } 
        public League League { get; set; } = null!;
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; } 
    }
}