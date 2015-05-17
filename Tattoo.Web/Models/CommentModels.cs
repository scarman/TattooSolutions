using System;
using System.ComponentModel.DataAnnotations;

namespace Tattoo.Web.Models
{
    public class CommentViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string CommentText { get; set; }

        public DateTime DateCreated { get; set; }

        public string ElementId { get; set; }

        public UserInfoCommentsViewModel Author { get; set; }
    }


    public class CommentFormModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string CommentText { get; set; }

        public DateTime DateCreated { get; set; }

        public string IdElement { get; set; }

        public string AuthorName { get; set; }

        public ElementViewModel Element { get; set; }

        public UserInfoCommentsViewModel Author { get; set; }

    }
}