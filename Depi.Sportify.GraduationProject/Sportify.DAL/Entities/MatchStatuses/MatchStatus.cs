using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.DAL.Entities.MatchStatuses
{
    public class MatchStatus:BaseEntity<int>
    {
       //Id
       public string StatusName { get; set; }
    }
}
