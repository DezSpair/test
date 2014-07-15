using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XmlWebService.Contracts.Models;

namespace XmlWebService.Contracts
{
    public interface IXmlService
    {
        XDocument XDocument { get; set; }
        List<string> GetAllTableName();
        List<TableModel> GetDataFromXDoc();
        XElement CreateReport(List<TableModel> tableModels);

    }
}
