using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EssentialTraining;

namespace EssentialTrainingTests
{
    [TestClass]
    public class AwesomeSauceTest
    {
        [TestMethod]
        public void IsSauceAwesomeTest()
        {
            var testInstance = new AwesomeSauce();
            testInstance.Sauces.Add("Yeet");
            testInstance.Sauces.Add("Assholi");
            Assert.IsTrue(testInstance.isSauceAwesome("Yeet"));
            Assert.IsFalse(testInstance.isSauceAwesome("Hot sauce"));
        }
    }
}
