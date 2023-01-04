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
    using PuppeteerSharp;

    public class Program
    {
        static void Main(string[] args)
        {
            string fullUrl = "https://orbitxch.com/customer/sport/1/competition/all";
            List<string> programmerLinks = new List<string>();

            var options = new ChromeOptions()
            {
                BinaryLocation = "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe"
            };

            options.AddArguments(new List<string>() { "disable-gpu" });
            
            var browser = new ChromeDriver(options);

            browser.Navigate().GoToUrl(fullUrl);
            browser.Manage().Window.Size = new System.Drawing.Size(1800, 1009);
            Thread.Sleep(8000);

           var test = browser.PageSource;
            //Console.WriteLine(test);
            var commingUpEvents = browser.FindElement(By.ClassName("js-coming-up-sport-region"));
            var matches = commingUpEvents.FindElements(By.ClassName("biab_table-wrapper"));
            foreach (var item in matches)
            {

                var time = item.FindElement(By.ClassName("biab_market-time")).Text;
                var teams = item.FindElement(By.ClassName("biab_market-title-team-names"));
                var teamSpan = teams.FindElements(By.TagName("span"));
                var team1 = teamSpan[0].Text;
                var team2 = teamSpan[1].Text;

                Console.WriteLine($"Time: {time}  {team1} vs {team2}");
            }
            browser.Quit();
           //Console.WriteLine(table.Text);
            
		}
    }
}
