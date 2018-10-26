using System.Web.Services;
using System.Web.Services.Protocols;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    /// Contains methods which convert a string to all uppercase or all lowercase.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class TextService : System.Web.Services.WebService
    {
        /// <summary>
        /// Converts a string to all uppercase or all lowercase.
        /// </summary>
        /// <param name="incoming">The string to be converted.</param>
        /// <param name="flag">A flag (1 or 2) that represents whether the string should be converted to all uppercase or all lowercase.</param>
        /// <returns>The converted string.</returns>
        [WebMethod]
        public string CaseConvert(string incoming, uint flag)
        {
            string result = string.Empty;

            if (incoming.Length == 0)
            {
                throw new SoapException("The length of the incoming string must be greater than 0.", Soap12FaultCodes.RpcBadArgumentsFaultCode);
            }
            else if (flag == 0 || flag > 2)
            {
                throw new SoapException("The flag must be either 1 or 2.", Soap12FaultCodes.RpcBadArgumentsFaultCode);
            }
            else
            {
                result = TextServiceLogic.CaseConvert(incoming, flag);
            }

            return result;
        }

        
    }

    /// <summary>
    /// Contains methods which converts a string to all uppercase or all lowercase.
    /// </summary>
    public static class TextServiceLogic
    {
        /// <summary>
        /// Converts a string to all uppercase or all lowercase.
        /// </summary>
        /// <param name="incoming">The string to be converted.</param>
        /// <param name="flag">A flag (1 or 2) that represents whether the string should be converted to all uppercase or all lowercase.</param>
        /// <returns>The converted string.</returns>
        public static string CaseConvert(string incoming, uint flag)
        {
            return flag == 1 ? incoming.ToUpper() : incoming.ToLower();
        }
    }
}
