using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Common.Enumerations;

namespace Tattoo.Data.Entities
{
    public class Selection
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual Element Element { get; set; }

        public virtual string UserId { get; set; }

        public virtual Guid ElementId { get; set; }

        public TypeSelection SelectionType { get; set; }

    }
}
