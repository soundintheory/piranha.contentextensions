using Piranha.Extend;
using Piranha.Models;
using SoundInTheory.Piranha.ContentExtensions.Areas.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Models
{
    [ContentGroup(Id = "ContentAreas", Title = "Content Areas", Icon = "fas fa-layer-group")]
    public abstract class ContentArea : GenericContent, IBlockContent
    {
        public IList<Block> Blocks { get; set; } = new List<Block>();
    }
}
