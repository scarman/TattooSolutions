using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tattoo.Data.Entities
{
    public class BodyZone
    {
        public Guid Id { get; set; }
        
        public string Zone { get; set; }
        
        public virtual ICollection<Element> Elements { get; set; } 
       }
}
