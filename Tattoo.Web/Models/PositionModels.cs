#region Using Directives

using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Tattoo.Common.Enumerations;

#endregion

namespace Tattoo.Web.Models
{
    public class PositionViewModel
    {
        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PositionStatus Status { get; set; }
    }

    public class PositionFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public PositionFormModel()
        {
            Id = Guid.NewGuid();
        }

        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public PositionStatus Status { get; set; }
    }

    public class TestFormModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public TestFormModel()
        {
            Id = Guid.NewGuid();
        }

        [ScaffoldColumn(false)]
        public Guid Id { get; set; }

        [AllowHtml]
        public string Content { get; set; }
    }
}