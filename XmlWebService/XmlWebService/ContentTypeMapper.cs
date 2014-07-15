using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;

namespace XmlWebService
{
    public class ContentTypeMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {

            if (contentType.Contains("text/xml") || contentType.Contains("application/xml"))
            {

                return WebContentFormat.Raw;

            }

            else
            {

                return WebContentFormat.Default;

            }
        }
    }
}