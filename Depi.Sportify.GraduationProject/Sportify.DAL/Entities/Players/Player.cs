using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sportify.DAL.Entities.Players
{
    public class Player : BaseEntity<int>
    {

        //Id
        public string Name { get; set; }
        public int Age { get; set; }
        public string Position { get; set; }


    }
}
