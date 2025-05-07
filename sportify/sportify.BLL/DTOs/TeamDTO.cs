using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.DTOs
{
    public class TeamDTO
    {
        public int Id { get; set; }

        [Required]

        [RegularExpression(@"^Team.*$", ErrorMessage = "Name must start with 'Team'.")]

        public string Name { get; set; } = "Team";

        public string Description { get; set; }

        public string LogoUrl { get; set; }
        
        public string Location { get; set; }

        public string FoundedYear { get; set; }

        public string StadiumName { get; set; }
    }
}
