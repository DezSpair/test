using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlWebService.Contracts.Enums;

namespace XmlWebService.Contracts.Models
{
    public class TableModel
    {
        public string TableName { get; set; }
        public List<RecordModels> Records { get; set; }
    }

    public class RecordModels
    {
        public List<RowModel> RowModels { get; set; }
        public RowStatus RowStatus { get; set; }
        public string ErrorMsg { get; set; }
    }

    public class RowModel
    {
        public string FieldName { get; set; }
        public string FieldType { get; set; }
        public string FieldValue { get; set; }

    }
}
