using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify.DAL.Entities
{
    public class MatchStatistic
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Match")]
        public int MatchId { get; set; }
        public virtual Match Match { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        [Required]
        public int Goals { get; set; }

        [Required]
        public int YellowCards { get; set; }

        [Required]
        public int RedCards { get; set; }
    }
}