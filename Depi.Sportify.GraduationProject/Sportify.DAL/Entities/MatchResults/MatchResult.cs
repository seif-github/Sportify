using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.DAL.Entities.MatchResults
{
    public class MatchResult : BaseEntity<int>

    {
        //Id
        public string ResultName { get; set; }

    }
}
