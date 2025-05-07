using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sportify.BLL.CustomModels
{
    public class TournamentModel
    {
        public int Id { get; set; }

        [DefaultValue("myTeam")]
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string LogoUrl { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public int Status { get; set; }
    }

}
