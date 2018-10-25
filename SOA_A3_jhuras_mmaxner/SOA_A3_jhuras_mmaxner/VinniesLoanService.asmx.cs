using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    /// Summary description for VinniesLoanService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class VinniesLoanService : System.Web.Services.WebService
    {

        [WebMethod]
        public float Loan(float principle, float rate, int payments)
        {
            float result = 0f;

            if (principle <= 0)
            {
                throw new FaultException("Principle must be greater than 0.", new FaultCode("ArgumentFault"));
            }
            else if (rate <= 0 || rate > 1)
            {
                throw new FaultException("Rate must be between 0 and 1.", new FaultCode("ArgumentFault"));
            }
            else
            {
                result = LoanCalculator.CalculateLoan(principle, rate, payments);
            }

            return result;
        }

        
    }
    public static class LoanCalculator
    {
        public static float CalculateLoan(float principle, float rate, int payments)
        {
            float amortized_rate = rate / 12.0f;
            return (float)Math.Round((float)(amortized_rate + (amortized_rate / (float)(Math.Pow(1 + amortized_rate, payments) - 1.0f))) * principle, 2, MidpointRounding.ToEven);
        }
    }
}
