using System;

namespace SoundInTheory.Piranha.ManagerExtensions.Lists.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SingletonAttribute : Attribute
    {
        public string Title { get; set; }

        public string Slug { get; set; }

        public bool ShowInMenu { get; set; }

        public string Icon { get; set; }
    }
}
