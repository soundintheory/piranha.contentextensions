using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Models
{
    public class ContentAreaType
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string Icon { get; set; }

        public string TypeId { get; set; }

        public string CLRType { get; set; }
    }
}
