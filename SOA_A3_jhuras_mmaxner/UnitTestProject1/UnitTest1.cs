﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SOA_A3_jhuras_mmaxner;

namespace UnitTestProject1
{
    [TestClass]
    public class TextCasingUnitTest
    {
        // test that the method can convert to upper case
        // input: a sentence, and a variety of symbols that shouldn;t change with case change
        // expected output: the same sentence all uppercase, non-letter characters are the same
        [TestMethod]
        public void ToUpperTest()
        {
            string OrginalCaseText = "All caps means shouting. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string UpperCaseText = "ALL CAPS MEANS SHOUTING. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string result = TextServiceLogic.CaseConvert(OrginalCaseText, 1);
            Assert.AreEqual(UpperCaseText, result);
        }

        // test that the method can convert to lower case
        // input: a sentence, and a variety of symbols that shouldn't change with case change
        // expected output: the same sentence all lowercase, non-letter characters are the same
        [TestMethod]
        public void ToLowerTest()
        {
            string OrginalCaseText = "LOweR CasE LetTErs ARe qUIet. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string UpperCaseText = "lower case letters are quiet. 1234567890!@#$%^&*()_+-={}[]|\\:;'\"~`";
            string result = TextServiceLogic.CaseConvert(OrginalCaseText, 2);
            Assert.AreEqual(UpperCaseText, result);
        }

        // test that the method works with an empty input
        // input: an empty string
        // expected output: an empty string
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
        // test that the method can calculate an normal loan value
        // input: a moderate principle, rate, and number of payments
        // expected output: the correct amount needed per payment
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

        // test that the method works when the principle is 0
        // input: a principle of 0, and a moderate rate and number of payments
        // expected output: 0
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

        // test that the method works when the rate is zero
        // note: per the formula used for this assignment, the expected value is NaN
        // input: a rate of zero, a moderate principle and number of payments
        // expected output: NaN
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

        // test that the method works when the payments are 0
        // input: a payments of zero, a moderate principle and rate
        // expected output: NaN
        [TestMethod]
        public void ZeroPayments()
        {
            float principle = 500;
            float rate = 0.08f;
            int payments = 0;
            string ExpectedAmount = "∞";
            float result = LoanCalculator.CalculateLoan(principle, rate, payments);
            Assert.AreEqual(ExpectedAmount, result.ToString("0.00"));
        }

        // test that the method works when the rate is very high
        // input: a very high rate, moderate principle, and moderate payments
        // expected output: the correct amount needed per payment
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

    [TestClass]
    public class IPResolverTest
    {
        // test for google.com
        [TestMethod]
        public void IPTest1()
        {
            string ip = "172.217.0.99";
            IPResolver.IPInfo expected = new IPResolver.IPInfo()
            {
                Country = "United States",
                Latitude = 37.75101m,
                Longitude = -97.822m
            };
            IPResolver.IPInfo result = IPResolver.GetInfo(ip).Result;
            Assert.AreEqual(expected, result);
        }

        // test for cool-math-games.com
        [TestMethod]
        public void IPTest2()
        {
            string ip = "185.53.179.6";
            IPResolver.IPInfo expected = new IPResolver.IPInfo()
            {
                Country = "Germany",
                Latitude = 51.2993m,
                Longitude = 9.490997m
            };
            IPResolver.IPInfo result = IPResolver.GetInfo(ip).Result;
            Assert.AreEqual(expected, result);
        }

        // test for canada.ca
        [TestMethod]
        public void IPTest3()
        {
            string ip = "167.40.79.24";
            IPResolver.IPInfo expected = new IPResolver.IPInfo()
            {
                Country = "Canada",
                Latitude = 43.6319m,
                Longitude = -79.3716m
            };
            IPResolver.IPInfo result = IPResolver.GetInfo(ip).Result;
            Assert.AreEqual(expected, result);
        }

        // test for paizo.com
        [TestMethod]
        public void IPTest4()
        {
            string ip = "70.35.123.53";
            IPResolver.IPInfo expected = new IPResolver.IPInfo()
            {
                City = "Redmond",
                Country = "United States",
                Latitude = 47.6718m,
                Longitude = -122.1232m,
                StateProvince = "WA"
            };
            IPResolver.IPInfo result = IPResolver.GetInfo(ip).Result;
            Assert.AreEqual(expected, result);
        }

        // test for a fake ip
        // expected output: not found
        [TestMethod]
        public void IPTest5()
        {
            string ip = "qwe.rt.yui.op";
            IPResolver.IPInfo expected = new IPResolver.IPInfo()
            {
                Organization = "Not Found"
            };
            IPResolver.IPInfo result = IPResolver.GetInfo(ip).Result;
            Assert.AreEqual(expected, result);
        }
    }
}
