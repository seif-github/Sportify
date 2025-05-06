using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        
        [StringLength(255)]
        public string Description { get; set; }
        
        [StringLength(255)]
        public string LogoUrl { get; set; }
        
        [StringLength(100)]
        public string Location { get; set; }
        
        [StringLength(50)]
        public string FoundedYear { get; set; }
        
        [StringLength(255)]
        public string StadiumName { get; set; }
        
        public int? ManagerUserId { get; set; }
        
        [ForeignKey("ManagerUserId")]
        public User ManagerUser { get; set; }
        
        public virtual ICollection<Player> Players { get; set; }
        public virtual ICollection<TeamTournament> TeamTournaments { get; set; }
        public virtual ICollection<User> Fans { get; set; }
    }
}