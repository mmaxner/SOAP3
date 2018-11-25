using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;
using SOA___Assignment_2___Web_Services;
using System.Net;
using System.Net.Sockets;
using System.Xml;

namespace SOA_A3_jhuras_mmaxner
{
    public class SOAPLogger
    {
        private string FileName;

		private static byte[] guid;
		private static string url = "http://127.0.0.1:8080/LoggingService";
		private static string log_action = "Log";
		private static string token_action = "GetToken";
		private static string service_namespace = "LoggingService";

		private static byte[] key = System.Text.Encoding.ASCII.GetBytes("di9ha67d2hdg1044");
		private static byte[] iv = System.Text.Encoding.ASCII.GetBytes("tommy_shelby8265");

		public  void LogThroughService()
		{
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("arg0", System.Convert.ToBase64String(RijndaelEncryptor.EncryptStringToBytes("Very bad for the teeth.", key, iv)));
			parameters.Add("arg1", System.Convert.ToBase64String(RijndaelEncryptor.EncryptStringToBytes(System.Convert.ToBase64String(guid), key, iv)));
			WebServiceFramework.CallWebService(url, log_action, parameters, service_namespace);
		}

        public SOAPLogger(string ServiceName)
        {
			Dictionary<string, string> parameters = new Dictionary<string, string>();
			parameters.Add("arg0", "\"" + GetLocalIPAddress() + "\"");
			string result = WebServiceFramework.CallWebService(url, token_action, parameters, "");
			XmlDocument envelope = new XmlDocument();
			envelope.LoadXml(result);
			result = envelope.GetElementsByTagName("return")[0].InnerText;
			guid = System.Text.Encoding.ASCII.GetBytes(RijndaelEncryptor.DecryptStringFromBytes(System.Convert.FromBase64String(result), key, iv));

			FileName = "C:/Windows/Temp/" + ServiceName + ".log";
        }

        public void LogFault(SoapException fault)
        {
            string data = DateTime.UtcNow.ToLongDateString() + " " + fault.Code + " " + fault.Message + " " + fault.InnerException + " " + fault.StackTrace;
            using (FileStream fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                fs.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            }
        }

        public void LogStuff(string stuff)
        {
            string data = DateTime.UtcNow.ToLongDateString() + " " + stuff;
            using (FileStream fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                Console.Write(fs);
                fs.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            }
        }

        public void LogException(Exception ex)
        {
            string data = DateTime.UtcNow.ToLongDateString() + " " + "Unexpected Exception" + " " + ex.Message + " " + ex.InnerException + " " + ex.StackTrace;
            using (FileStream fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                fs.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            }
        }

		public static string GetLocalIPAddress()
		{
			var host = Dns.GetHostEntry(Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == AddressFamily.InterNetwork)
				{
					return ip.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}
	}
}