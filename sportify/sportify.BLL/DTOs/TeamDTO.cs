using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class TeamDTO
    {
        public int TeamID { get; set; }
        [Display (Name = "Team Name")]
        public string Name { get; set; } = null!;
        public int? Wins { get; set; }
        public int? Losses { get; set; }
        public int? Draws { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int? TotalMatchesPlayed { get; set; }
        public int? Points { get; set; }
        public int LeagueID { get; set; }
        [Display(Name = "Team Image")]
        public string ImageUrl
        {
            get => _imageUrl ?? "/assets/default-team.png";
            set => _imageUrl = value;
        }
        private string? _imageUrl;
    }
}
