using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.DAL.Entities
{
    public class Score:BaseEntity<int>
    {
        //Id

        public int Points { get; set; }

    }
}
