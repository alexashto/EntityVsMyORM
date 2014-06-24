using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM
{
    public class FieldAttribute : Attribute
    {
        public string Name { get; set; }
        public string Type { get; set; }

        public FieldAttribute(string name, string type)
        {
            Name = name;
            Type = type;
        }
    }
}
