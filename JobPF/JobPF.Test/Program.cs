using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using JobPF.Business;
using System.Threading;

namespace JobPF.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            JobCrawler crawler = new JobCrawler(appSettings["consumerKey"], appSettings["consumerSecret"], appSettings["token"], appSettings["secret"]);
            crawler.Start();

            while (true)
            {
                Thread.Sleep(1000);
            }
            //TweetManager t = new TweetManager();
            //t.SendTweet("ceci est un test avec une URL : http://www.sharepointofview.fr/sylvain/");
           
           // Logger.WriteError("hello world error");
        }
    }
}
