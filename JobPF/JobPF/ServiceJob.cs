using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using JobPF.Business;
using System.Configuration;

namespace JobPF
{
    public partial class ServiceJob : ServiceBase
    {
        private JobCrawler crawler;
        public ServiceJob()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var appSettings = ConfigurationManager.AppSettings;
            JobCrawler crawler = new JobCrawler(appSettings["consumerKey"], appSettings["consumerSecret"], appSettings["token"], appSettings["secret"]);
            try
            {
                crawler.Start();
                Logger.WriteMessage("JobCrawler démarré");
            }
            catch (Exception e)
            {
                Logger.WriteError("Erreur au démarrage de jobcrawler : " + e.ToString());
            }
        }

        protected override void OnStop()
        {
            if (crawler != null)
            {
                try
                {
                    crawler.End();
                    Logger.WriteMessage("JobCrawler arrété");
                }
                catch (Exception e)
                {
                    Logger.WriteError("Erreur à l'arret de jobcrawler : " + e.ToString());
                }
            }
        }
    }
}
