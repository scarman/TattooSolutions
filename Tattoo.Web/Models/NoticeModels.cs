using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tattoo.Common.Enumerations;

namespace Tattoo.Web.Models
{
    public class NoticeViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public NoticeStatus Status { get; set; }
    }
    
    public class NoticeFormModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public NoticeStatus Status { get; set; }
    }
}