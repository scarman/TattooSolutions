#region Using Directives

using System.Collections.Generic;

#endregion

namespace Tattoo.Web.Models
{
    public class CountryData
    {
        public IEnumerable<dynamic> Countries { get; set; }
    }

    public class StateData
    {
        public IEnumerable<dynamic> States { get; set; }
    }
}