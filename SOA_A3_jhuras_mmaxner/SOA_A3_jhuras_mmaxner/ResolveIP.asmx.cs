using SOA___Assignment_2___Web_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    /// Gets geographical information from an ip address from another web service.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class IPResolvingService : System.Web.Services.WebService
    {

        SOAPLogger logger;
        public IPResolvingService()
        {
            logger = new SOAPLogger("IPResolvingService");
        }
        /// <summary>
        /// Checks for a valid formatted IP address and calls another webservice for the IPInfo results.
        /// </summary>
        /// <param name="ip">The IP address to fetch geographical information from.</param>
        /// <returns>An IPInfo struct containing geographical information based on the IP address.</returns>
        /// 
        [WebMethod]
        public IPResolver.IPInfo GetInfo(string ip)
        {
            try
            {
                if (!isValidIP(ip))
                {
                    throw new SoapException("Incorrect IP address format.", Soap12FaultCodes.RpcBadArgumentsFaultCode);
                }
                return IPResolver.GetInfo(ip).Result;
            }
            catch (SoapException ex)
            {
                logger.LogFault(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                throw new Exception("Unknown Internal Error");
            }
        }  

        private bool isValidIP(string ip)
        {
            return new Regex(@"\b(?:(?:[0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\.){3}(?:[0-9]|[1-9][0-9]|1[0-9][0-9]|2[0-4][0-9]|25[0-5])\b").Match(ip).Success;
        }
    }

    /// <summary>
    /// Contains methods which makes a call to another web service to get geographical information based on an ip address.
    /// </summary>
    public static class IPResolver
    {
        /// <summary>
        /// Makes a call to another web service to get geographical information based on an ip address.
        /// </summary>
        /// <param name="ipAddress">The IP address to fetch geographical information from.</param>
        /// <returns>An IPInfo struct containing geographical information based on the IP address.</returns>
        public static async System.Threading.Tasks.Task<IPInfo> GetInfo(string ipAddress)
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

        /// <summary>
        /// Creates and returns an IPInfo based on an XML string containing IPInfo properties.
        /// </summary>
        /// <param name="xmlString">The XML string containing IPInfo data.</param>
        /// <returns>An IPInfo struct containing geographical information based on the IP address.</returns>
        private static async System.Threading.Tasks.Task<IPInfo> fillIPInfoFromXMLAsync(string xmlString)
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
        /// <param name="s">The string to be used for the stream.</param>
        /// <returns>A stream based on the string provided.</returns>
        //https://stackoverflow.com/questions/1879395/how-do-i-generate-a-stream-from-a-string
        private static Stream GenerateStreamFromString(string s)
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
