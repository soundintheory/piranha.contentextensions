using Piranha.AttributeBuilder;
using Piranha.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Models
{
    [ContentGroup(Id = "GenericContentAreas", Title = "Generic Content Areas", Icon = "fas fa-layer-gruop")]
    [ContentType(Id = "GenericContentArea", Title = "Generic Content Area", UseExcerpt = false, UsePrimaryImage = false)]
    public class GenericContentArea : ContentArea
    {

    }
}
