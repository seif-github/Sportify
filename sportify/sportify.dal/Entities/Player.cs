using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify.DAL.Entities
{
    public class Player
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم اللاعب مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم لا يمكن أن يزيد عن 100 حرف")]
        public string Name { get; set; }

        [ForeignKey("Team")]
        public int TeamId { get; set; }
        public virtual Team Team { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
    }
}