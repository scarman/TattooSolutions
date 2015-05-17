#region Using Directives

using System;
using System.Collections.Generic;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Data.Entities
{
    public class Position
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PositionStatus Status { get; set; }

        //public virtual ICollection<ApplicationDetails> Details { get; set; }
    }
}