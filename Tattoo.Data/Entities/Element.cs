using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Common.Enumerations;

namespace Tattoo.Data.Entities
{
    public class Element
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public int Ranking { get; set; }

        public int CountLike { get; set; }

        public int CountOriginal { get; set; }

        public int CountVisits { get; set; }

        public int CountFollows { get; set; }

        public DateTime DateCreated { get; set; }

        public string Url { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public TypeElement Type { get; set; }

        public virtual BodyZone Zone { get; set; }

        public virtual ICollection<Selection> Selection { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
