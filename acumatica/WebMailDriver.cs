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
            //ввести логин и пароль и нажать кнопку "войти"
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
            var submitButton=driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"mailbox__auth__button\"]"));//?
            submitButton.Click();
            System.Threading.Thread.Sleep(1000);//
            return "Authentication form operations successful";
            }

        public string sendMessage()
            {
            //нажать на кнопку "отправить письмо".
            try
                {
                var submitButton = driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"b-toolbar__left\"]/div/div/div[2]/div/a/span"));
                submitButton.Click();
                System.Threading.Thread.Sleep(1000);//
                }
            catch (OpenQA.Selenium.NoSuchElementException exc)
                {
                return "Authentication failed or 'send' button not available";    
                }
            //Задать получателя
            try
                {
                var toField = driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"compose__header__content\"]/div[2]/div[2]/div[1]/input[3]"));
                toField.SendKeys(System.Configuration.ConfigurationManager.AppSettings["username"] + "@" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"]+"\n");
                }
            catch (Exception exc)
                {
                System.Threading.Thread.Sleep(1000);//
                var toField = driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"compose__header__content\"]/div[2]/div[2]/div[1]/input[3]"));
                toField.SendKeys(System.Configuration.ConfigurationManager.AppSettings["username"] + "@" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"] + "\n");
                }
            //Нажать на кнопку "Отправить"
            var sendButton = driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"b-toolbar__right\"]/div[3]/div/div/div[1]/div/span"));
            sendButton.Click();
            System.Threading.Thread.Sleep(1000);//
            try
                {
                //На случай возникновения вопросов вида а вы уверены, что хотите отправить пустое письмо?
                var confirmForm = driver.FindElement(OpenQA.Selenium.By.ClassName("is-submit_empty_message_in"));
                var confirmButton = confirmForm.FindElement(OpenQA.Selenium.By.ClassName("confirm-ok"));
                confirmButton.Click();
                }
            catch (Exception exc)
                {
                }
            //Выходим и снова заходим, чтобы увидеть входящие...
            driver.Close();
            driver = WebDriverFactory.getWebDriver(System.Configuration.ConfigurationManager.AppSettings["browser"]);
           
            driver.Url = "https://" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
            driver.Navigate();
            this.Auth();
            System.Threading.Thread.Sleep(1000);//
            //проверяем наличие
            var mailItems = driver.FindElements(OpenQA.Selenium.By.ClassName("b-datalist__item__link"));
            foreach (var mailIten in mailItems)
                {
                string title = mailIten.GetAttribute("title");
                string address = System.Configuration.ConfigurationManager.AppSettings["username"] + "@" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
                if (title.Contains(address))   return "Message recived";
                }
            return "Something wrong";
            }
        }
    }
