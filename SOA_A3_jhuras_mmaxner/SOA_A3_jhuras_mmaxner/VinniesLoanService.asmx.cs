using System;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace SOA_A3_jhuras_mmaxner
{
    /// <summary>
    ///     Contains methods that calculate the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments.
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class VinniesLoanService : System.Web.Services.WebService
    {
        SOAPLogger logger;
        public VinniesLoanService()
        {
            logger = new SOAPLogger("VinniesLoanService");
        }

        /// <summary>
        ///     Calculates the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments.
        /// </summary>
        /// <param name="principle">Initial amount of the loan.</param>
        /// <param name="rate">The annual interest rate as a decimal. 15% = 0.15.</param>
        /// <param name="payments">The number of monthly payments that will be made.</param>
        /// <returns>The amount to be paid for each payment towards the principle.</returns>
        [WebMethod]
        public float LoanPayment(float principle, float rate, int payments)
        {
            float result = 0f;

            logger.LogStuff("principle: " + principle + ", rate: " + rate + ", payments: " + payments);

            if (principle <= 0)
            {
                throw new SoapException("Principle must be greater than 0.", Soap12FaultCodes.RpcBadArgumentsFaultCode);
            }
            else if (rate <= 0 || rate > 1)
            {
                throw new SoapException("Rate must be between 0 and 1.", Soap12FaultCodes.RpcBadArgumentsFaultCode);
            }
            else
            {
                result = LoanCalculator.CalculateLoan(principle, rate, payments);
            }

            return result;
        }

        
    }

    /// <summary>
    ///     Contains methods that calculate the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments.
    /// </summary>
    public static class LoanCalculator
    {
        /// <summary>
        ///     Calculates the amount needed per payment to pay off a loan, given the amount, interest rate, and number of monthly payments.
        /// </summary>
        /// <param name="principle">Initial amount of the loan.</param>
        /// <param name="rate">The annual interest rate as a decimal. 15% = 0.15.</param>
        /// <param name="payments">The number of monthly payments that will be made.</param>
        /// <returns>The amount to be paid for each payment towards the principle.</returns>
        public static float CalculateLoan(float principle, float rate, int payments)
        {
            float amortized_rate = rate / 12.0f;    // the rate is annual, but payments are monthly. 
            return (float)Math.Round((float)(amortized_rate + (amortized_rate / (float)(Math.Pow(1 + amortized_rate, payments) - 1.0f))) * principle, 2, MidpointRounding.ToEven);
        }
    }
}
