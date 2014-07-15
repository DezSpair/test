using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlWebService.Contracts;
using XmlWebService.Contracts.Enums;
using XmlWebService.Contracts.Models;

namespace XmlWebService.BLL
{
    public class XmlService : IXmlService
    {
        public XDocument XDocument { get; set; }

        public List<string> GetAllTableName()
        {
            return XDocument.Descendants("name").Select(e => e.Value.ToString()).ToList();
        }

        public List<TableModel> GetDataFromXDoc()
        {
            var tableList = new List<TableModel>();

            var tables = XDocument.Descendants("name");
            foreach (var e in tables)
            {
                var table = new TableModel { TableName = e.Value.ToString() };
                var recordList = new List<RecordModels>();
                var nextNode = e.NextNode as XElement;

                if (nextNode == null || nextNode.Name.ToString().ToLower() != "data") continue;
                var rows = nextNode.Elements("row");
                foreach (var r in rows)
                {
                    var record = new RecordModels();
                    var rowModels = new List<RowModel>();
                    var elements = r.Elements();
                    var rowsList = elements.Select(el => new RowModel
                    {
                        FieldName = el.Name.ToString(),
                        FieldType = el.Attribute("type").Value.ToString(),
                        FieldValue = el.Value
                    }).ToList();
                    rowModels.AddRange(rowsList);
                    record.RowModels = rowModels;
                    recordList.Add(record);
                }
                table.Records = recordList;
                tableList.Add(table);
            }
            return tableList;
        }

        public XElement CreateReport(List<TableModel> tableModels)
        {

            
            var record = new List<RecordModels>();
            record.AddRange(tableModels.SelectMany(p => p.Records).Where(p => p.RowStatus == RowStatus.Error));
            var all = tableModels.Select(p => p.Records.Count).Sum();
            var result =  new XElement("Report", 
                new XElement("AllRow", all),
                new XElement("RowWasInserted", (all - record.Count)),
                new XElement("ErrorCount", record.Count),
                new XElement("Errors", record.Select(err => new XElement("error", err.ErrorMsg))));
            return result;
        }
    }
}
