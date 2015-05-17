#region Using Directives

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Tattoo.Web.Models
{
    public class DashboardViewModel
    {
        public int TotalApplications { get; set; }

        public int ProcessedApplications { get; set; }

        public int PendingApplications
        {
            get { return TotalApplications - ProcessedApplications; }
        }

        public IEnumerable<object[]> FormsByStatus { get; set; }

        public int StatusCount
        {
            get { return FormsByStatus.Count(); }
        }
    }
}