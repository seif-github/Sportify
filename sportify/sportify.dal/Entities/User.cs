using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sportify.DAL.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم المستخدم مطلوب")]
        [StringLength(100, ErrorMessage = "اسم المستخدم طويل جدًا")]
        public string Username { get; set; }

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [StringLength(100, ErrorMessage = "البريد الإلكتروني طويل جدًا")]
        [EmailAddress(ErrorMessage = "صيغة البريد الإلكتروني غير صحيحة")]
        public string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(255, ErrorMessage = "كلمة المرور طويلة جدًا")]
        public string PasswordHash { get; set; }

        [Required(ErrorMessage = "الدور مطلوب")]
        [StringLength(50, ErrorMessage = "الدور طويل جدًا")]
        public string Role { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        public int? FavoriteTeamId { get; set; }

        [ForeignKey("FavoriteTeamId")]
        public virtual Team FavoriteTeam { get; set; }
    }
}