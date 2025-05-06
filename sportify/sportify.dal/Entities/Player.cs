using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class Player
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; }
    }
}