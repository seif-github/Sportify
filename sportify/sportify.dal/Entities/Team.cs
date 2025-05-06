using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify.DAL.Entities
{
    public class Team
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم الفريق مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم لا يمكن أن يزيد عن 100 حرف")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "الوصف لا يمكن أن يزيد عن 255 حرف")]
        public string Description { get; set; }

        [StringLength(255, ErrorMessage = "رابط اللوجو غير صحيح")]
        public string LogoUrl { get; set; }

        [StringLength(100, ErrorMessage = "الموقع الجغرافي طويل جدًا")]
        public string Location { get; set; }

        [StringLength(50, ErrorMessage = "سنة التأسيس غير صحيحة")]
        public string FoundedYear { get; set; }

        [StringLength(255, ErrorMessage = "اسم الملعب طويل جدًا")]
        public string StadiumName { get; set; }

        public int? ManagerUserId { get; set; }

        [ForeignKey("ManagerUserId")]
        public virtual User ManagerUser { get; set; }

        public virtual ICollection<Player> Players { get; set; } = new HashSet<Player>();
        public virtual ICollection<TeamTournament> TeamTournaments { get; set; } = new HashSet<TeamTournament>();
        public virtual ICollection<User> Fans { get; set; } = new HashSet<User>();
    }
}