using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Protocols;
using System.IO;
using System.Text;

namespace SOA_A3_jhuras_mmaxner
{
    public class SOAPLogger
    {
        private string FileName;
        public SOAPLogger(string ServiceName)
        {
            FileName = ServiceName + ".log";
        }

        public void LogFault(SoapException fault)
        {
            string data = DateTime.UtcNow.ToLongDateString() + " " + fault.Code + " " + fault.Message + " " + fault.InnerException + " " + fault.StackTrace;
            using (FileStream fs = File.Open(FileName, FileMode.OpenOrCreate))
            {
                fs.Write(Encoding.ASCII.GetBytes(data), 0, data.Length + 1);
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
                fs.Write(Encoding.ASCII.GetBytes(data), 0, data.Length + 1);
            }
        }
    }
}