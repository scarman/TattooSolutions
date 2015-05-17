using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tattoo.Common.Enumerations;

namespace Tattoo.Data.Entities
{
    public class Notice
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public NoticeStatus Status { get; set; }
    }
}
