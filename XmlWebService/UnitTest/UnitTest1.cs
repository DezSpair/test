using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlWebService;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Service1 _service1;

        public UnitTest1()
        {
            _service1 = new Service1();
        }
        [TestMethod]
        public void TestValidXml()
        {
            var report = GetReport(@"./xml/valid.xml");
            var all = report.Element("AllRow").Value;
            var insert = report.Element("RowWasInserted").Value;
            var error = report.Element("ErrorCount").Value;
            bool result = Convert.ToInt32(all) == 2 && Convert.ToInt32(insert) == 2 && Convert.ToInt32(error) == 0;
            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void TestErrorXml()
        {
            var report = GetReport(@"./xml/error.xml");
            var all = report.Element("AllRow").Value;
            var insert = report.Element("RowWasInserted").Value;
            var error = report.Element("ErrorCount").Value;
            bool result = Convert.ToInt32(all) == 2 && Convert.ToInt32(insert) == 0 && Convert.ToInt32(error) == 2;
            Assert.AreEqual(true, result);
            
        }

        [TestMethod]
        public void TexsMultiXml()
        {
            var report = GetReport(@"./xml/multixml.xml");
            var all = report.Element("AllRow").Value;
            var insert = report.Element("RowWasInserted").Value;
            var error = report.Element("ErrorCount").Value;
            bool result = Convert.ToInt32(all) == 10 && Convert.ToInt32(insert) == 8 && Convert.ToInt32(error) == 2;
            Assert.AreEqual(true, result);

        }

        private XElement GetReport(string path)
        {
            
            MemoryStream ms = new MemoryStream();
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(ms, xws))
            {
                XDocument doc = XDocument.Load(path);
                doc.WriteTo(xw);
            }

            return _service1.DoWork(ms);
        }
    }
}
