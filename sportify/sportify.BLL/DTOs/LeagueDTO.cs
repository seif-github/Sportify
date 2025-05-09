using sportify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class LeagueDTO
    {
        public int LeagueID { get; set; }
        public string Name { get; set; } = null!;
        public string SportType { get; set; } = null!;
        public string OrganizerID { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public int DurationBetweenMatches { get; set; }
        public int NumberOfTeams { get; set; }
        public bool IsCompleted { get; set; }
        public string? ImageUrl { get; set; }
        public ApplicationUser Organizer {  get; set; }


    }
}
