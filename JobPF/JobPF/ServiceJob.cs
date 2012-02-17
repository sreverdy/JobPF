﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using JobPF.Business;

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
            crawler = new JobCrawler();
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
