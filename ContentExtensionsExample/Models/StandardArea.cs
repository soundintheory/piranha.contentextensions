using Piranha.AttributeBuilder;
using Piranha.Extend;
using Piranha.Extend.Fields;
using SoundInTheory.Piranha.ContentExtensions.Areas.Attributes;
using SoundInTheory.Piranha.ContentExtensions.Areas.Models;

namespace ContentExtensionsExample.Models
{
    [ContentArea(Title = "Standard Area", Icon = "fas fa-smile", Description = "Test Area")]
    [ContentType(Id = "FooterArea", Title = "Footer Area", UseExcerpt = false, UsePrimaryImage = false)]
    public class StandardArea : ContentArea
    {
        [Region]
        public StringField TestField { get; set; }
    }
}
