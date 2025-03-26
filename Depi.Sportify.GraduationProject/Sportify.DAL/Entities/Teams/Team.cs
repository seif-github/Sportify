using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.DAL.Entities.Teams
{
    public class Team:BaseEntity<int>
    {
        //int  ID 
        public string Name { get; set; }
        public string Coach { get; set; }
        public int Ranking { get; set; }
        //I donot know what is CreatedAt 
        public DateOnly CreationAt { get; set; }


    }
}
