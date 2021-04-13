using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ParrotNameTests
{
    public class HomeworkTests
    {
        public ChromeDriver driver;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
        }

        private const string email = "test@mail.ru";
        private const string siteUrl = "https://qa-course.kontur.host/selenium-practice";
        private readonly By boyRadioButtonLocator = By.Id("boy");
        private readonly By girlRadioButtonLocator = By.Id("girl");
        private readonly By emailInputLocator = By.Name("email");
        private readonly By buttonLocator = By.Id("sendMe");
        private readonly By emailResultLocator = By.ClassName("your-email");
        private readonly By resultTextLocator = By.ClassName("result-text");
        private readonly By errorMessageLocator = By.ClassName("form-error");
        private readonly By anotherEmailLinkLocator = By.LinkText("указать другой e-mail");

        [Test]
        public void ComputerSite_CheckGirl_FillFormWithEmail_Success()
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(girlRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашей девочки на e-mail:",
                driver.FindElement(resultTextLocator).Text, "Список имен сформирован для девочки");
            Assert.AreEqual(email, driver.FindElement(emailResultLocator).Text, "Создали заявку на список имен для девочки на указанный e-mail");
        }

        [Test]
        public void ComputerSite_CheckBoy_FillFormWithEmail_Success()
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(boyRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Хорошо, мы пришлём имя для вашего мальчика на e-mail:",
                driver.FindElement(resultTextLocator).Text, "Список имен сформирован для мальчика");
            Assert.AreEqual(email, driver.FindElement(emailResultLocator).Text, "Создали заявку на список имен для мальчика на указанный e-mail");
        }

        [Test]
        public void ComputerSite_ChangeGender_EmailIsNotCleared()
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(boyRadioButtonLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(girlRadioButtonLocator).Click();
            
            Assert.AreEqual(email, driver.FindElement(emailInputLocator).GetAttribute("value"), "После смены пола животного поле e-mail не очистилось");
        }

        [Test]
        public void ComputerSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле очистилось");
            Assert.IsTrue(driver.FindElements(anotherEmailLinkLocator).Count == 0, "Не исчезла ссылка для ввода другого e-mail");
        }

        [Test]
        public void ComputerSite_LeaveEmailFormEmpty_EmptyInputErrorMessage()
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(string.Empty);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Введите email", driver.FindElement(errorMessageLocator).Text, "Уведомление о незаполненном поле e-mail");
        }
        
        [Test]
        public void ComputerSite_TooLongEmailLength_EmailLengthErrorMessage()
        {
            var name = RandomGenerate(313);
            var email = $"{name}@mail.ru";

            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Некорректный email", driver.FindElement(errorMessageLocator).Text, "Уведомление о некорректном e-mail");
        }

        [Test]
        [TestCase("@mail.ru")]
        [TestCase("zlatmail.ru")]
        [TestCase("zlat@mail.")]
        [TestCase("zlat@.ru")]
        public void ComputerSite_FillWithInvalidEmail_IncorrectEmailErrorMessage(string email)
        {
            driver.Navigate().GoToUrl(siteUrl);

            driver.FindElement(emailInputLocator).SendKeys(email);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Некорректный email", driver.FindElement(errorMessageLocator).Text, "Уведомление о некорректном e-mail");
        }

        private static string RandomGenerate(int length)
        {
            var stringBuilder = new StringBuilder();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrsqtuvwxyz";
            var random = new Random();
            for (var i = 0; i < length; i++)
                stringBuilder.Append(chars[random.Next(chars.Length)]);

            return stringBuilder.ToString();
        }
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}


