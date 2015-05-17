using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tattoo.Data.Entities;

namespace Tattoo.Web.Models
{
    public class BodyZoneViewModels
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Zone { get; set; }

        public ICollection<Element> Elements { get; set; }
    }

    public class BodyZoneFormModels
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Zone { get; set; }

        public ICollection<Element> Elements { get; set; }
    }
}
