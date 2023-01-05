namespace ScraperConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AngleSharp;
    using AngleSharp.Scripting.JavaScript;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Support.UI;
    using PuppeteerSharp;

    public class Program
    {
        static void Main(string[] args)
        {
            string fullUrl = "https://orbitxch.com/customer/sport/1/competition/all";

            var options = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            options.AddArguments(new List<string>() { "disable-gpu" });

            var browser = new ChromeDriver(options);

            browser.Navigate().GoToUrl(fullUrl);
            browser.Manage().Window.Size = new System.Drawing.Size(1800, 1009);
            //browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(8);
            Thread.Sleep(4000);

            //Finding and pressing today matches button
            var todayMatchesButton = browser.FindElement(By.XPath("//*[@id='biab_content']/div/div[3]/div/div[1]/div[2]/div[1]/div/div[1]/span[2]/span/span/span[3]"));
            todayMatchesButton.Click();
            Thread.Sleep(3000);

            //Select view by competition
            var toggleMenu = browser.FindElement(By.XPath("//*[@id='ui-id-2-button']"));
            toggleMenu.Click();
            Thread.Sleep(600);
            var compleatedButton = browser.FindElement(By.XPath("//*[@id='ui-id-5']"));
            compleatedButton.Click();
            Thread.Sleep(3000);

            //Getting information for comming up events
            //TODO: Getting information for in play events.
            
            var isNextButtonActive = true;
            while (isNextButtonActive)
            {
                var commingUpEvents = browser.FindElement(By.ClassName("js-inplay-sports-list"));
                var matchesWrapper = commingUpEvents.FindElements(By.ClassName("biab_inplay-sport-wrapper"));

                foreach (var item in matchesWrapper)
                {
                    var leagueName = item.FindElement(By.ClassName("biab_inplay-sport-item-title"));

                    Console.WriteLine(leagueName.Text);

                    var matches = item.FindElements(By.ClassName("biab_table-wrapper"));


                    foreach (var item2 in matches)
                    {
                    
                        var time = item2.FindElement(By.ClassName("biab_market-time")).Text;
                        var teams = item2.FindElement(By.ClassName("biab_market-title-team-names"));
                        var teamSpan = teams.FindElements(By.TagName("span"));
                        var team1 = teamSpan[0].Text;
                        var team2 = teamSpan[1].Text;
                        var back1 = item2.FindElement(By.ClassName("js-back-0")).FindElement(By.ClassName("js-odds")).Text;
                        var backX = item2.FindElement(By.ClassName("js-back-1")).FindElement(By.ClassName("js-odds")).Text;
                        var back2 = item2.FindElement(By.ClassName("js-back-2")).FindElement(By.ClassName("js-odds")).Text;


                        Console.WriteLine($"Time: {time}  {team1} Odds: {back1} vs {team2} Odds: {back2} X Odds: {backX}");
                    }
                }
                

                browser.FindElement(By.LinkText("Next")).Click();
                Thread.Sleep(2000);

                //Always return true
                isNextButtonActive = browser.FindElement(By.LinkText("Next")).Enabled;

            }
            
            browser.Quit();

        }
    }
}
