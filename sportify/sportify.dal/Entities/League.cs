using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace sportify.DAL.Entities
{
    public class League
    {
        public int LeagueID { get; set; }  
        public string Name { get; set; } = null!;
        public string SportType { get; set; } = null!;
        [ForeignKey("Organizer")]
        public string OrganizerID { get; set; } = null!; 
        public DateTime StartDate { get; set; }
        public int DurationBetweenMatches { get; set; }
        public int NumberOfTeams { get; set; }
        public bool IsCompleted { get; set; }  
        public IEnumerable<Team> Teams { get; set; } = new List<Team>();  
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        public ApplicationUser Organizer { get; set; } = null!;
    }
}