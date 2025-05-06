using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify.DAL.Entities
{
    public class Tournament
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم البطولة مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم لا يمكن أن يزيد عن 100 حرف")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "الوصف لا يمكن أن يزيد عن 255 حرف")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "رابط اللوجو غير صحيح")]
        public string LogoUrl { get; set; }

        [Required(ErrorMessage = "تاريخ البدء مطلوب")]
        [Column(TypeName = "datetime2")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "تاريخ الانتهاء مطلوب")]
        [Column(TypeName = "datetime2")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "الحالة مطلوبة")]
        public TournamentStatus Status { get; set; } = TournamentStatus.Upcoming;

        public virtual ICollection<TeamTournament> TeamTournaments { get; set; } = new HashSet<TeamTournament>();
        public virtual ICollection<Match> Matches { get; set; } = new HashSet<Match>();
    }
    public enum TournamentStatus
    {
        Upcoming,
        Active,
        Completed,
        Cancelled
    }
}