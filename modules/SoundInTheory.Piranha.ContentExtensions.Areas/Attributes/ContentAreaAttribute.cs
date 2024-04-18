using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ContentAreaAttribute : Attribute
    {
        /// <summary>
        /// Gets/sets the optional title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets/sets the optional description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets/sets the menu icon class
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ContentAreaAttribute() : base()
        {
        }
    }
}
