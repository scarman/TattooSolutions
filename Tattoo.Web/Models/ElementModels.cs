#region Using Directives

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Tattoo.Common.Enumerations;
using Tattoo.Common.Strings;
using Tattoo.Data.Entities;
using Tattoo.Web.Core.Extensions;

#endregion

namespace Tattoo.Web.Models
{
    public class ElementViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public string DescriptionShorted
        {
            get { return Description.Shorten(160); }
        }

        public int Ranking { get; set; }

        public int CountLike { get; set; }

        public int CountOriginal { get; set; }

        public int CountVisits { get; set; }

        public int CountFollows { get; set; }

        //Saber si votó o no por un elemento en específico
        public bool Like { get; set; }
        public bool Original { get; set; }
        public bool Follows { get; set; }

        public DateTime DateCreated { get; set; }

        public string Url { get; set; }
        public string SizedPicture(int width)
        {
            return string.Format("{0}?h={1}", Url, width);
        }

        public string SizedPicture(int height, int width)
        {
            return string.Format("{0}?h={1}&w={2}", Url, width, height);
        }

        public UserInfoViewModel Author { get; set; }

        public string Avatar { get; set; }

        public TypeElement Type { get; set; }

        public BodyZoneViewModels Zone { get; set; }

        public IEnumerable<Comment> Comments { get; set; }

        public int TotalComments
        {
            get { return Comments.Count(); }
        }

    }

    public class ElementFormModel
    {
        public ElementFormModel()
        {
            Id = Guid.NewGuid();
        }

        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Field_Description")]
        public string Description { get; set; }

        public int Ranking { get; set; }

        public int CountLike { get; set; }

        public int CountOriginal { get; set; }

        public int CountVisits { get; set; }

        public int CountFollows { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(ResourceType = typeof(Resources), Name = "Field_Url")]
        public HttpPostedFileBase Url { get; set; }

        public TypeElement Type { get; set; }

        [Display(ResourceType = typeof(Resources), Name = "Field_Zone")]
        public string ZoneId { get; set; }
    }

    public class ElementEditFormModel
    {
        public ElementEditFormModel()
        {
            Id = Guid.NewGuid();
        }

        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Description { get; set; }

        public TypeElement Type { get; set; }

        public string Url { get; set; }

        public string SizedPicture(int width)
        {
            return string.Format("{0}?h={1}", Url, width);
        }

        public string SizedPicture(int height, int width)
        {
            return string.Format("{0}?h={1}&w={2}", Url, width, height);
        }
    }

}