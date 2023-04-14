using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkStar.Api.Engine.Attributes.Seed
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SeedObjectAttribute : Attribute
    {
        public string TemplateDirectory { get; set; }

        public SeedObjectAttribute(string templateDirectory)
        {
            TemplateDirectory = templateDirectory;
        }
    }
}
