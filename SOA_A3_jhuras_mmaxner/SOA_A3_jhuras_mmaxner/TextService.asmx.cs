using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    /// Summary description for TextService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TextService : System.Web.Services.WebService
    {

        [WebMethod]
        public string Case(string incoming, uint flag)
        {
            string result = string.Empty;

            if (incoming.Length == 0)
            {
                throw new FaultException("The length of the incoming string must be greater than 0.", new FaultCode("ArgumentFault"));
            }
            else if (flag == 0 || flag > 2)
            {
                throw new FaultException("The flag must be either 1 or 2.", new FaultCode("ArgumentFault"));
            }
            else
            {
                result = TextServiceLogic.CaseConvert(incoming, flag);
            }

            return result;
        }

        
    }

    public static class TextServiceLogic
    {
        public static string CaseConvert(string incoming, uint flag)
        {
            return flag == 1 ? incoming.ToUpper() : incoming.ToLower();
        }
    }
}
