using SOA___Assignment_2___Web_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml;
using System.ServiceModel;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    /// Summary description for ResolveIP
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ResolveIP : System.Web.Services.WebService
    {

        [WebMethod]
        public IPInfo Resolve(string ip)
        {
            return GetInfo(ip).Result;
        }

        private async System.Threading.Tasks.Task<IPInfo> GetInfo(string ipAddress)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ipAddress", ipAddress);
            parameters.Add("licenseKey", "0");
            string result = string.Empty;

            try
            {
                result = WebServiceFramework.CallWebService("http://ws.cdyne.com/ip2geo/ip2geo.asmx", "ResolveIP", parameters, "http://ws.cdyne.com/");
            }
            catch (Exception e)
            {
                throw new FaultException("Something went wrong accessing http://ws.cdyne.com.\n" + e.Message, new FaultCode("ConnectionFault"));
            }
            
            return await fillIPInfoFromXMLAsync(result);
        }

        private async System.Threading.Tasks.Task<IPInfo> fillIPInfoFromXMLAsync(string xmlString)
        {
            IPInfo ipinfo = new IPInfo();
            IPInfoProperty? ipinfoProperty = null;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Async = true;

            using (Stream resultStream = GenerateStreamFromString(xmlString))
            {
                using (XmlReader reader = XmlReader.Create(resultStream, settings))
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                switch (reader.Name)
                                {
                                    case "City":
                                        ipinfoProperty = IPInfoProperty.City;
                                        break;
                                    case "StateProvince":
                                        ipinfoProperty = IPInfoProperty.StateProvince;
                                        break;
                                    case "Country":
                                        ipinfoProperty = IPInfoProperty.Country;
                                        break;
                                    case "Organization":
                                        ipinfoProperty = IPInfoProperty.Organization;
                                        break;
                                    case "Latitude":
                                        ipinfoProperty = IPInfoProperty.Latitude;
                                        break;
                                    case "Longitude":
                                        ipinfoProperty = IPInfoProperty.Longitude;
                                        break;
                                    default:
                                        ipinfoProperty = null;
                                        break;
                                }
                                break;
                            case XmlNodeType.Text:
                                if (ipinfoProperty != null)
                                {
                                    string resultText = await reader.GetValueAsync();
                                    switch (ipinfoProperty)
                                    {
                                        case IPInfoProperty.City:
                                            ipinfo.City = resultText;
                                            break;
                                        case IPInfoProperty.StateProvince:
                                            ipinfo.StateProvince = resultText;
                                            break;
                                        case IPInfoProperty.Country:
                                            ipinfo.Country = resultText;
                                            break;
                                        case IPInfoProperty.Organization:
                                            ipinfo.Organization = resultText;
                                            break;
                                        case IPInfoProperty.Latitude:
                                            ipinfo.Latitude = decimal.Parse(resultText);
                                            break;
                                        case IPInfoProperty.Longitude:
                                            ipinfo.Longitude = decimal.Parse(resultText);
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return ipinfo;
        }

        /// <summary>
        ///     Produces a stream from a basic string.
        /// </summary>
        //https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        private Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public struct IPInfo
        {
            public string City;
            public string StateProvince;
            public string Country;
            public string Organization;
            public decimal Latitude;
            public decimal Longitude;
        }

        enum IPInfoProperty
        {
            City,
            StateProvince,
            Country,
            Organization,
            Latitude,
            Longitude
        }
    }
}
