using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class LeagueDTO
    {
        public int LeagueID { get; set; }
        [Required]
        [Display(Name = "League Name")]
        public string Name { get; set; } = null!;
        [Display(Name = "Sport Type")]
        public string OrganizerID { get; set; } = null!;
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Duration Between Rounds")]
        public int DurationBetweenMatches { get; set; }
        [Display(Name = "Number of Teams")]
        public int NumberOfTeams { get; set; }
        [Display(Name = "Play Each Team Twice")]
        public bool RoundRobin { get; set; }

        [Display(Name = "Completed")]
        public bool IsCompleted { get; set; }
        [Display(Name = "League Image")]
        
        public string? imageUrl;

        //public ApplicationUser Organizer {  get; set; }


    }
}
