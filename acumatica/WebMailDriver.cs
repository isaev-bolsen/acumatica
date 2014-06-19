using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acumatica
    {
    class WebMailDriver
        {
        OpenQA.Selenium.IWebDriver driver;
        public WebMailDriver()
            {
            driver = WebDriverFactory.getWebDriver(System.Configuration.ConfigurationManager.AppSettings["browser"]);
            driver.Url = "https://" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
            driver.Navigate();
            }
        public string Auth()
            {
            var loginBox = driver.FindElement(OpenQA.Selenium.By.Name("Login"));
            var passwordBox = driver.FindElement(OpenQA.Selenium.By.Name("Password"));
            loginBox.Clear();
            loginBox.SendKeys(System.Configuration.ConfigurationManager.AppSettings["username"]);
            passwordBox.Clear();
            string password = System.Configuration.ConfigurationManager.AppSettings["password"];
            if (password != "")
                {
                passwordBox.SendKeys(password);
                }
            else
                {
                Console.WriteLine("password?");
                passwordBox.SendKeys(Console.ReadLine());
                }
            var submitButton=driver.FindElement(OpenQA.Selenium.By.ClassName("mailbox__auth__button"));//?
            submitButton.Click();
            return "Success";
            }
        }
    }
