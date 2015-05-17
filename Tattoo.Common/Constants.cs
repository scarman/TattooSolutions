#region Using Directives

using System.Collections;

#endregion

namespace Tattoo.Common
{
    public static class Constants
    {
        public const string ApplicationName = "TattooWebApp";

        public const string AuthCookieName = "_ASPMVCX.TattooAuthToken";

        public const string CultureCookieName = "_ASPMVCUI.TattooCultureSetting";

        public const string ApplicationSessionObjName = "_ASPSESSION.TattooApplicationObject";

        public const byte DefaultPageSize = 5;

        public const string DefaultAdminNickName = "Admin";

        public const string DefaultAdminUserName = "admin@tattoo.com";

        public const string DefaultAdminPassword = "tattoo!1";

        public static IEnumerable PageSizes
        {
            get
            {
                return new[]
                {
#if DEBUG
                    new {Name = "1", Value = 1},
#endif
                    new {Name = "5", Value = 5},
                    new {Name = "10", Value = 10},
                    new {Name = "25", Value = 25},
                    new {Name = "50", Value = 50}
                };
            }
        }

        public static IEnumerable LastGrades
        {
            get
            {
                return new[]
                {
                    new {Name = "7", Value = 7},
                    new {Name = "8", Value = 8},
                    new {Name = "9", Value = 9},
                    new {Name = "10", Value = 10},
                    new {Name = "11", Value = 11},
                    new {Name = "12", Value = 12}
                };
            }
        }
    }
}