using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelTestTask;
using ModelTestTask.Interfaces;
using System;

namespace UnitTestModelProject
{
    [TestClass]
    public class UnitTest1
    {
        private const decimal resultConvert = (decimal)8.5896;
        [TestMethod]
        public void TestMethodConvertCurrency_10USDToEUR_resultConvert()
        {
            decimal codeFromValue = (decimal)70.8623;
            decimal codeFromNominal = 1;
            decimal codeToValue = (decimal)82.4979;
            decimal codeToNominal = 1;
            decimal value = 10;
            decimal result = 0;
            Model model = new Model();
            result = model.convertCurrency(codeFromValue, codeFromNominal, codeToValue, codeToNominal, value);
            Assert.AreEqual(resultConvert, result);
        }
        private const decimal resultRUB = (decimal)82.4979;
        [TestMethod]
        public void TestMethodPaymentCourseRUB_1EURinRUB_resultRUB()
        {
            decimal currentCodeValue = (decimal)82.4979;
            decimal currentCodeNominal = 1;
            Model model = new Model();
            decimal rubcourse = model.PaymentCourseRUB(currentCodeValue, currentCodeNominal);
            Assert.AreEqual(resultRUB, rubcourse);
        }
        private const decimal resultUSD = (decimal)1.1642;
        [TestMethod]
        public void TestMethodPaymentCourseEUR_1EURinUSD_resultUSD()
        {
            decimal codeUsdValue = (decimal)70.8623;
            decimal codeUsdNominal = 1;
            decimal rubcourse = (decimal)82.4979;
            Model model = new Model();
            decimal usdcourse = model.PaymentCourseUSD(codeUsdValue, codeUsdNominal, rubcourse);
            Assert.AreEqual(resultUSD, usdcourse);
        }
    }
}
