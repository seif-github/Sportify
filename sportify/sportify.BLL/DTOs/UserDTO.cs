using System.ComponentModel.DataAnnotations;

namespace sportify.BLL.DTOs
{
    public class UserDTO
    {
        public string Id { get; set; }
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}