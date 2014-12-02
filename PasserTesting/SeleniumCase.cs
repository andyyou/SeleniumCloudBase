using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using System.Linq;


namespace PasserTesting
{
    [TestClass]
    public class SeleniumCase
    {
        private IWebDriver _driver;
        private const String WEB_SITE = "http://192.168.1.106:1337";
        private const String TAEGET_EVENT = "/event/545844937c1abd3b08e09039";
        private const String PAYMENT_EVENT = "/payment/545844937c1abd3b08e09039";
        private const String SECOND_EVENT = "/event/52aaec9ce2d3960a3b000001";
        public TestContext TestContext
        {
            get;
            set;
        }

        [TestMethod]
        public void VisitPasser()
        {
            TestContext.BeginTimer("Begin View Index");
            _driver.Navigate().GoToUrl(WEB_SITE);
            TestContext.EndTimer("Begin View Index");
            var title = _driver.Title;
            Assert.IsTrue(title.Equals("Passer 夢想通行證"));
        }

        [TestMethod]
        public void VisitEventDetial()
        {
            TestContext.BeginTimer("Begin View event detail");
            _driver.Navigate().GoToUrl(WEB_SITE + SECOND_EVENT);
            TestContext.EndTimer("Begin View event detail");
            var eventTitle = _driver.FindElement(By.TagName("h3")).Text;
            Assert.IsTrue(eventTitle.Equals("進擊的太白粉之毛巾加購"));
        }

        [TestMethod]
        public void LoginPasser()
        {
            _driver.Navigate().GoToUrl(WEB_SITE);
            
            TestContext.BeginTimer("Begin Login");
            _driver.FindElement(By.Id("login")).Click();
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("loginAccount")).Clear();
            _driver.FindElement(By.Id("loginAccount")).SendKeys("test@wavinfo.com");
            _driver.FindElement(By.Id("loginPassword")).Clear();
            _driver.FindElement(By.Id("loginPassword")).SendKeys("12345678");
            Thread.Sleep(1000);
            _driver.FindElement(By.Id("loginButton")).Click();
            Thread.Sleep(1000);
            _driver.FindElement(By.LinkText("會員資料")).Click();
            TestContext.EndTimer("Begin Login");

            var elementValue = _driver.FindElement(By.CssSelector(".form-control-static:first-child"));
            Assert.IsTrue(elementValue.Text.Equals("test@wavinfo.com"));       

            /*
            TestContext.BeginTimer("Select Seat");
            _driver.FindElement(By.XPath("//div[6]/a/div/div")).Click();
            _driver.FindElement(By.Id("buy")).Click();
            for (int second = 0; ; second++)
            {
                if (second >= 60) Assert.Fail("timeout");
                try
                {
                    if (IsElementPresent(By.Id("54696b7f822fe1e9264cc3bc"))) break;
                }
                catch (Exception)
                { }
                Thread.Sleep(1000);
            }
            _driver.FindElement(By.Id("54696b7f822fe1e9264cc3bc")).Click();
            _driver.FindElement(By.Id("54696b7f822fe1e9264cc3bd")).Click();
            _driver.FindElement(By.LinkText("下一步")).Click();
            TestContext.EndTimer("Select Seat");
            
            var elementValue = _driver.FindElement(By.CssSelector("input#email"));
            Assert.IsTrue(elementValue.GetAttribute("value").Equals("test@wavinfo.com"));       
            */
        }

        [TestMethod]
        public void PurchaseProcess()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            // string base64Guid = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", String.Empty);
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var guid = new string(
            Enumerable.Repeat(chars, random.Next(4,12))
              .Select(s => s[random.Next(s.Length)])
              .ToArray());
            String testAccount = String.Format("{0}@test.gg", guid);
            String testPassword = "12345678";
            String registerURL = String.Format("{0}/bot-signup?email={1}&password={2}",WEB_SITE, testAccount , testPassword);
            _driver.Navigate().GoToUrl(registerURL);
            Thread.Sleep(2000);
            _driver.Navigate().GoToUrl(WEB_SITE);
            _driver.FindElement(By.Id("login")).Click();
            // Thread.Sleep(1000);
            _driver.FindElement(By.Id("loginAccount")).Clear();
            _driver.FindElement(By.Id("loginAccount")).SendKeys(testAccount);
            _driver.FindElement(By.Id("loginPassword")).Clear();
            _driver.FindElement(By.Id("loginPassword")).SendKeys(testPassword);
           //  Thread.Sleep(1000);
            _driver.FindElement(By.Id("loginButton")).Click();
            Thread.Sleep(1000);
            _driver.Navigate().GoToUrl(WEB_SITE + TAEGET_EVENT);
            _driver.Navigate().GoToUrl(WEB_SITE + PAYMENT_EVENT);
            // Thread.Sleep(4000); // Waiting for map loading
            var rand = new Random();
            int x = Convert.ToInt32(_driver.FindElement(By.Id("n11")).GetAttribute("x"));
            int y = Convert.ToInt32(_driver.FindElement(By.Id("n11")).GetAttribute("y"));
            
            var isChoosed = false;
            do
            {
                var xpath = String.Format("//*[contains(@id,'n{0}')]", rand.Next(0, 2063));
                Actions e = new Actions(_driver);
                var svg = _driver.FindElement(By.XPath(xpath));
                e.MoveToElement(svg)
                 .MoveByOffset(1, 1)
                 .Click()
                 .Perform();
                Thread.Sleep(200);
                isChoosed = _driver.FindElements(By.ClassName("note")).Count > 0;
            } while (!isChoosed);
           
            
            // Thread.Sleep(2000);
            _driver.FindElement(By.Id("stepClick1")).Click();
            var ticketCount = _driver.FindElements(By.ClassName("a-cart")).Count;
            // Thread.Sleep(1000);
            _driver.FindElement(By.CssSelector("#receiveForm input#name")).SendKeys("TEST-Milly");
            _driver.FindElement(By.CssSelector("#receiveForm input#mobileNumber")).SendKeys("0911911911");
            //  _driver.FindElement(By.Id("city")).Click();
            var selectCity = new SelectElement(_driver.FindElement(By.Id("city")));
            selectCity.SelectByText("臺北市");
           
            // _driver.FindElement(By.Id("cityArea")).Click();
            var selectCityArea = new SelectElement(_driver.FindElement(By.Id("cityArea")));
            selectCityArea.SelectByText("中山區");
   
            _driver.FindElement(By.CssSelector("#receiveForm input#street")).SendKeys("Taipei City Chengde Rd., Datong Dist");
            _driver.FindElement(By.ClassName("termCheck")).Click();
            
            IList<IWebElement> dataFromFields = _driver.FindElements(By.CssSelector("#dataForm input[type='text']"));
            foreach (var input in dataFromFields)
            {
                input.SendKeys("0911911911");
            }

            Thread.Sleep(500);
            _driver.FindElement(By.Id("stepClick2")).Click();
            Thread.Sleep(500);
            var finishMessage = _driver.FindElement(By.ClassName("finish")).Text;
            _driver.FindElement(By.Id("finishClick")).Click();
            
            Assert.IsTrue(ticketCount == 1 && finishMessage.Equals("恭喜你購買完成")); 
        }

        [TestMethod]
        public void FillOutRegisterData()
        {
            Assert.IsTrue(true);
        }


        private bool IsElementPresent(By by)
        {
            try
            {
                _driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


        #region Additional test attributes
        [TestInitialize]
        public void SetupTestSuite()
        {
            Console.WriteLine("Test init called: {0}");
            _driver = new FirefoxDriver();
           
        }

        public void CleanupTestSuite()
        {
            _driver.Quit();
        }
        #endregion
    }
}
