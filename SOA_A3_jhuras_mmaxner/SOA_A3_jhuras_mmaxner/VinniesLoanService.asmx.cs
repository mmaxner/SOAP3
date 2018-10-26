using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.ServiceModel;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    ///     Calculates the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments
    /// </summary>
    /// <param name="principle">Initial amount of the loan</param>
    /// <param name="rate">The annual interest rate as a decimal. 15% = 0.15</param>
    /// <param name="payments">The number of monthly payments that will be made</param>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class VinniesLoanService : System.Web.Services.WebService
    {

        [WebMethod]
        public float LoanPayment(float principle, float rate, int payments)
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
        /// <summary>
        ///     Calculates the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments
        /// </summary>
        /// <param name="principle">Initial amount of the loan</param>
        /// <param name="rate">The annual interest rate as a decimal. 15% = 0.15</param>
        /// <param name="payments">The number of monthly payments that will be made</param>
        /// <returns></returns>
        public static float CalculateLoan(float principle, float rate, int payments)
        {
            float amortized_rate = rate / 12.0f;    // the rate is annual, but payments are monthly. 
            return (float)Math.Round((float)(amortized_rate + (amortized_rate / (float)(Math.Pow(1 + amortized_rate, payments) - 1.0f))) * principle, 2, MidpointRounding.ToEven);
        }
    }
}
