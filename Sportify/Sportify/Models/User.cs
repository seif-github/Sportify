using Microsoft.AspNetCore.Identity;

namespace Sportify.Models
{
    public enum Role
    {
        user =0,
        admin=1
    }
    public class User : IdentityUser
    {
        public Role Role { get; set; }
        public List<UserLeague> UserLeagues { get; set; } = new();
        public List<Notification> Notifications { get; set; }

    }
}
