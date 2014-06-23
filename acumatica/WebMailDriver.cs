using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace acumatica
    {
    class WebMailDriver
        {
        private bool isLogEnabled=false;
        private System.IO.StreamWriter logStream;
        OpenQA.Selenium.IWebDriver driver;

        private void log(string message)
            {
            Console.WriteLine(message);
            if (isLogEnabled) logStream.WriteLine(message);
            }
        
        public WebMailDriver()
            {
            try
                {
                isLogEnabled = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["isLogEnabled"]);
                if (isLogEnabled)
                    {
                    System.IO.FileInfo logFile = new System.IO.FileInfo("WebMail.log");
                    logStream = logFile.CreateText();
                    logStream.AutoFlush = true;
                    }
                }
            catch (Exception exc)
            {
            Console.WriteLine("Logging init exception: " + exc.Message);
            }
            log(DateTime.Now.ToString());
            driver = WebDriverFactory.getWebDriver(System.Configuration.ConfigurationManager.AppSettings["browser"]);
            }
            
        public void NavigateToDestination()
            {
            driver.Url = "https://" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
            log("Browser directed to " + driver.Url);
            driver.Navigate();
            }

        public string Auth()
            {
            //ввести логин и пароль и нажать кнопку "войти"
            log("Trying to get 'login' field...");
            var loginBox = driver.FindElement(OpenQA.Selenium.By.Name("Login"));
            log("Trying to get 'password' field...");
            var passwordBox = driver.FindElement(OpenQA.Selenium.By.Name("Password"));
            loginBox.Clear();
            loginBox.SendKeys(System.Configuration.ConfigurationManager.AppSettings["username"]);
            log("Username inputed");
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
            log("Password inputed");
            log("Trying to get'login' button...");
            var submitButton=driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"mailbox__auth__button\"]"));//?
            submitButton.Click();
            log("Navigating to 'inbox'");
            return "Authentication form operations successful";
            }

        public string sendMessage()
            {
            var wait =new OpenQA.Selenium.Support.UI.WebDriverWait(driver, new TimeSpan(0, 0, 30));

            //нажать на кнопку "отправить письмо".
            try
                {
                var ComposeButton = wait.Until(d=>d.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"b-toolbar__left\"]/div/div/div[2]/div/a/span")));
                ComposeButton.Click();
                log("Navigating to 'compose'");
                System.Threading.Thread.Sleep(1000);
                }
            catch (Exception exc)
                {
                log("Authentication failed or 'compose' button not available");
                return "Authentication failed or 'compose' button not available";    
                }
            //Задать получателя
            log("Trying to set destination address...");
            try
                {
                var waitToField = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, new TimeSpan(0, 0, 30));
                OpenQA.Selenium.IWebElement toField = waitToField.Until(d => d.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"compose__header__content\"]/div[2]/div[2]/div[1]/input[3]"))); //*[@id="compose__header__content"]/div[2]/div[2]/div[1]/input[3]
                toField.SendKeys(System.Configuration.ConfigurationManager.AppSettings["username"] + "@" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"] + "\n");
                log("Destination set");
                }
            catch (OpenQA.Selenium.NoSuchElementException exc)
                {
                log("Can't set destination");
                }
            
            //Нажать на кнопку "Отправить"
            log("Trying to find 'send' button");
            var sendButton = driver.FindElement(OpenQA.Selenium.By.XPath("//*[@id=\"b-toolbar__right\"]/div[3]/div/div/div[1]/div/span"));
            sendButton.Click();
            try
                {
                //На случай возникновения вопросов вида а вы уверены, что хотите отправить пустое письмо?
                //var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, new TimeSpan(0, 0, 60));

                var confirmForm = wait.Until(d=> d.FindElement(OpenQA.Selenium.By.ClassName("is-submit_empty_message_in")));
                log("Yes,  i need to send empty message...");
                var confirmButton = confirmForm.FindElement(OpenQA.Selenium.By.ClassName("confirm-ok"));
                confirmButton.Click();
                }
            catch (Exception exc)
                {
                }
            log("Message sent");
            //Выходим и снова заходим, чтобы увидеть входящие...
            log("Reopening browser...");
            driver.Close();
            driver = WebDriverFactory.getWebDriver(System.Configuration.ConfigurationManager.AppSettings["browser"]);
           
            driver.Url = "https://" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
            driver.Navigate();
            log("Browser directed to " + driver.Url);
            this.Auth();
            //проверяем наличие
            log("Trying to find message in inbox...");
            System.Threading.Thread.Sleep(1000);
            var waitMailItems = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, new TimeSpan(0, 1, 0));
            var mailItems = waitMailItems.Until(d=>d.FindElements(OpenQA.Selenium.By.ClassName("b-datalist__item__link")));
            foreach (var mailIten in mailItems)
                {
                string title = mailIten.GetAttribute("title");
                string address = System.Configuration.ConfigurationManager.AppSettings["username"] + "@" + System.Configuration.ConfigurationManager.AppSettings["mailerAddress"];
                if (title.Contains(address))
                    {
                    log("Message recived!");
                    return "Message recived";
                    }
                }
            log("Message not found");
            return "Something wrong";
            }
        }
    }
