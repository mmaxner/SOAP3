using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOA_A3_jhuras_mmaxner;

namespace UnitTestProject1
{
    [TestClass]
    public class TextCasingUnitTest
    {
        [TestMethod]
        public void ToUpperTest()
        {
            string OrginalCaseText = "All caps means shouting. No exceptions 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string UpperCaseText = "ALL CAPS MEANS SHOUTING. NO EXCEPTIONS 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string result = TextServiceLogic.CaseConvert(OrginalCaseText, 1);
            Assert.AreEqual(UpperCaseText, result);
        }
        [TestMethod]
        public void ToLowerTest()
        {
            string OrginalCaseText = "LOweR CasE LetTErs ARe qUIet. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string UpperCaseText = "lower case letters are quiet. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string result = TextServiceLogic.CaseConvert(OrginalCaseText, 2);
            Assert.AreEqual(UpperCaseText, result);
        }
        [TestMethod]
        public void EmptyInputTest()
        {
            string OrginalCaseText = "";
            string UpperCaseText = "";
            string result = TextServiceLogic.CaseConvert(OrginalCaseText, 1);
            Assert.AreEqual(UpperCaseText, result);
        }

    }

    [TestClass]
    public class LoanCalculatorUnitTest
    {
        [TestMethod]
        public void NormalTest()
        {
            float principle = 2018;
            float rate = 0.15f;
            int payments = 12;
            float ExpectedAmount = 182.14f;
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount.ToString("0.00"), result.ToString("0.00"));  
        }

        [TestMethod]
        public void ZeroPrincipleTest()
        {
            float principle = 0;
            float rate = 0.15f;
            int payments = 15;
            float ExpectedAmount = 0;
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount.ToString("0.00"), result.ToString("0.00"));  
        }

        [TestMethod]
        public void ZeroRateTest()
        {
            float principle = 99;
            float rate = 0f;
            int payments = 1;
            string ExpectedAmount = "NaN";
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount, result.ToString("0.00"));
        }

        [TestMethod]
        public void Low()
        {
            float principle = 500;
            float rate = 0.08f;
            int payments = 0;
            string ExpectedAmount = "∞";
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount, result.ToString("0.00"));
        }

        [TestMethod]
        public void HighInterestTest()
        {
            float principle = 10000;
            float rate = 1.0f;
            int payments = 24;
            float ExpectedAmount = 976.32f;
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount.ToString("0.00"), result.ToString("0.00"));
        }

       
    }
}
