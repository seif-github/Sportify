using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Models
{
    public class Tournament
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [StringLength(255)]
        public string LogoUrl { get; set; }

        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }
        
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }
        
        [Required]
        public TournamentStatus Status { get; set; } = TournamentStatus.Upcoming;
        
        public virtual ICollection<TeamTournament> TeamTournaments { get; set; }
        public virtual ICollection<Match> Matches { get; set; }
    }
    
    public enum TournamentStatus
    {
        Upcoming,
        Active,
        Completed,
        Cancelled
    }
}