using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JobPF.Business;
using System.Threading;

namespace JobPF.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            JobCrawler crawler = new JobCrawler();
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
