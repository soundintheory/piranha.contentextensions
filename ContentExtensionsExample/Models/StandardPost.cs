using Piranha.AttributeBuilder;
using Piranha.Models;

namespace ContentExtensionsExample.Models
{
    [PostType(Title = "Standard post")]
    public class StandardPost : Post<StandardPost>
    {
    }
}