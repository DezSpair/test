using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace XmlWebService
{
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "Push",
               RequestFormat = WebMessageFormat.Xml,
               ResponseFormat = WebMessageFormat.Xml,
               BodyStyle = WebMessageBodyStyle.Bare)]
        XElement DoWork(Stream xml);
    }

  
}
