using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tattoo.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public string CommentText { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Element Element { get; set; }
    }
}
