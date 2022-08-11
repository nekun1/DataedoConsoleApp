using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Models
{
    class ImportedObject : ImportedObjectBaseClass
    {
        public string Schema;

        public string ParentName;
        public string ParentType { get; set; }

        public string DataType { get; set; }
        public string IsNullable { get; set; }

        public double NumberOfChildren;

        public ImportedObject(string type, string name, string schema, string parentName, string parentType, string dataType, string isNullable)
        {
            Type = type;
            Name = name;
            Schema = schema;
            ParentName = parentName;
            ParentType = parentType;
            DataType = dataType;
            IsNullable = isNullable;
        }
    }
}
