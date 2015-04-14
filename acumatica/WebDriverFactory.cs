using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace acumatica
    {
    class WebDriverFactory
        {
        public static OpenQA.Selenium.IWebDriver getWebDriver(string BrowserName)
            {
            BrowserName=BrowserName.ToLower();
            if ("chrome" == BrowserName) return new OpenQA.Selenium.Chrome.ChromeDriver();
            if ("safari" == BrowserName) return new OpenQA.Selenium.Safari.SafariDriver();
            if ("firefox" == BrowserName || "fire fox" == BrowserName)
                {
                //OpenQA.Selenium.Firefox.FirefoxProfile FP = new OpenQA.Selenium.Firefox.FirefoxProfile();
                //FP.Clean();
                //FP.SetPreference("app.update.auto", false);
                return new OpenQA.Selenium.Firefox.FirefoxDriver();
                }
            if ("ie" == BrowserName || "internet explorer" == BrowserName || "infernet exploder" == BrowserName) return new OpenQA.Selenium.IE.InternetExplorerDriver();
            throw new Exception("Unknown browser: " + BrowserName);
            }
        }
    }
