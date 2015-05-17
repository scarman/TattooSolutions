#region Using Directives

using System;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PagedList.Mvc;
using Tattoo.Web.Models;

#endregion

namespace Tattoo.Web
{
    public static class AppUtilities
    {
        public static PagedListRenderOptions PagedListOptions
        {
            get
            {
                return new PagedListRenderOptions
                {
                    DisplayLinkToFirstPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToLastPage = PagedListDisplayMode.IfNeeded,
                    DisplayLinkToPreviousPage = PagedListDisplayMode.Always,
                    DisplayLinkToNextPage = PagedListDisplayMode.Always,
                    DisplayLinkToIndividualPages = false,
                    DisplayPageCountAndCurrentLocation = true,
                    DisplayEllipsesWhenNotShowingAllPageNumbers = false,
                    MaximumPageNumbersToDisplay = 3
                };
            }
        }

        public static string ToCountry(this string countryCode)
        {
            if (String.IsNullOrEmpty(countryCode))
                return String.Empty;

            var path = HostingEnvironment.MapPath("~/Content/JsonData/countries.json");
            if (path == null)
                return countryCode;

            var reader = new JsonTextReader(new StreamReader(path));

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto
            };

            var serializer = JsonSerializer.Create(settings);

            var countryData = (CountryData) serializer.Deserialize(reader, typeof (CountryData));

            foreach (var jObject in countryData.Countries.Cast<JObject>())
            {
                JToken token;
                if (jObject.TryGetValue(countryCode, out token))
                    return token.ToObject<string>();
            }

            return countryCode;
        }

        public static string ToState(this string stateCode, string countryCode)
        {
            if (String.IsNullOrEmpty(stateCode) || String.IsNullOrEmpty(countryCode))
                return String.Empty;

            var path = HostingEnvironment.MapPath("~/Content/JsonData/states.json");
            if (path == null)
                return stateCode;

            var reader = new JsonTextReader(new StreamReader(path));

            var settings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Auto
            };

            var serializer = JsonSerializer.Create(settings);

            var stateData = (StateData) serializer.Deserialize(reader, typeof (StateData));

            foreach (var jObject in stateData.States.Cast<JObject>())
            {
                JToken token;
                if (jObject.TryGetValue(countryCode, out token))
                {
                    var subToken = token.Value<JToken>();
                    foreach (var num in subToken.Children().Cast<JObject>())
                    {
                        var obj = num.Children().Children().Cast<JObject>().ToList<JObject>()[0];
                        try
                        {
                            if (obj.GetValue("code").ToObject<string>() == stateCode)
                                return obj.GetValue("name").ToObject<string>();
                        }
                        catch (Exception)
                        {
                            return stateCode;
                        }
                    }
                }
            }

            return stateCode;
        }
    }
}