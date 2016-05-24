using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prolliance.Membership.DataPersistence
{
    public class ExtensionField
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public ColumnType Type { get; set; }

        public int Length { get; set; } 

        public string Defaultval { get; set; }

        public string Des { get; set; }
 
    }
  public enum ColumnType
    {
        Number,
        String,
        Date
    }
}
