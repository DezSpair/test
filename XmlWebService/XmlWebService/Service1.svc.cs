using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XmlWebService.BLL;
using XmlWebService.Contracts;
using XmlWebService.Contracts.Enums;
using XmlWebService.Contracts.Models;
using XmlWebService.Data;

namespace XmlWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        private const string DbName = @"D:\1\test.db";
        private readonly IDataRepository _dataRepository;
        private readonly IXmlService _xmlService;

        public Service1()
        {
            _dataRepository = new DataService();
            _xmlService = new XmlService();
        }
        public XElement DoWork(Stream xml)
        {
            xml.Position = 0;
            var s = string.Empty;
            using (StreamReader reader1 = new StreamReader(xml, Encoding.UTF8))
            {
                 s =  reader1.ReadToEnd();
            }
            
           

            XDocument xDoc = XDocument.Parse(s);
            _xmlService.XDocument = xDoc;

            List<TableModel> res = _xmlService.GetDataFromXDoc();
            _dataRepository.CheckOrCreateDb(DbName);
            _dataRepository.CheckTable(res, DbName);
            _dataRepository.AddRecord(DbName, ref res);


            return _xmlService.CreateReport(res);
        }
    }
}
