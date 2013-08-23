using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Web;


///<tr align=left>
/// <td>
///     <a href="/SefiWeb/SefiOffres.nsf/vOffreWeb/F8CF8500FA8941D50A2579A500093DB6?OpenDocument">
///     <img src="/SefiWeb/SefiPublic.nsf/web/im_bonhomme_bleu.gif" border=0>
///         <font class=titreBleu>
///             <b>
///                 <u>626871 : Chauffeur de poids lourd</u>
///             </b>
///        </font>
///     </a>
///     <br>
///     <font class=textPetit>Date d'effet : 15/02/2012</font>
///     <br>
///     <font class=titreBleu>Vous serez responsable des livraisons d'un secteur donné chez les commerçants : magasins, supermarchés, snacks et restaurants.</font>
///     <br>
///     <font class=textPetit>Lieu : TAHITI - 
///     </font>
///     <br><br>
/// </td>
/// </tr>
/// <tr align=left><td><a href="/SefiWeb/SefiOffres.nsf/vOffreWeb/01880F9F6BDD9EBE0A2579A500605A96?OpenDocument"><img src="/SefiWeb/SefiPublic.nsf/web/im_bonhomme_bleu.gif" border=0><font class=titreBleu><b><u>626873 : Médiateur/Médiatrice pédagogique</u></b></font></a><br><font class=textPetit>Date d'effet : 15/02/2012</font><br><font class=titreBleu>Recherche d'un médiateur documentaire (H/F) :<br>Missions principales du poste :<br>Le médiateur documentaire participe à la mise à disposition de l'offre documentaire proposée au public, en développant les outils de formation, d'information et de communication utiles et nécessaires à cette approbation.<br>Activités principales :<br>Concevoir une nouvelle politique de service à la bibliothèque.<br>- Concevoir des cours de méthodologie documentaire en ligne sur la plateforme de ressources pédagogiques de l'université : Conception de cours à destination des étudiants de niveau Licence, Master, Doctorat, PE1 et PE2; Organisation modulaire, progressive et interactive du contenu pédagogique.<br>- Initier et participer aux actions de formation à la méthodologie documentaire en présentiel, destinées aux usagers.<br>Participer aux missions de service public de la bibliothèque, dont l'accueil, le renseignement, l'orientation, ...</font><br><font class=textPetit>Lieu : TAHITI - </font><br><br></td></tr>


///<div class="liste_offres">
/// <div style="margin-left:10px;" class="moyen gras">
///     <a id="mb20" href="detail_offre.php?ID=18&from=index&popup=mb20" class="mb nolink" rel="width:800,height:500">
///         Employé Polyvalent
///     </a>
/// </div>
/// <div style="margin-left:10px;" class="moyen gras">
///    <a id="mb21" href="detail_offre.php?ID=17&from=index&popup=mb21" class="mb nolink" rel="width:800,height:500">
///         Assistant De Gestion Et Paie
///    </a>
/// </div>

namespace JobPF.Business
{
    public class JobCrawler
    {
        public List<Job> _Jobs;
        private string _XmlFilePath = @"c:\job_data.xml";
        public const int KeepNb = 50;
        System.Timers.Timer timer;
        public TweetManager _TweetManager;

        public JobCrawler(string xmlFilePath, string consumerKey, string consumerSecret, string token, string secret)
        {
            _XmlFilePath = xmlFilePath;
            _TweetManager = new TweetManager(consumerKey, consumerSecret, token, secret);
        }

        public void Start()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Job>));
            if (File.Exists(_XmlFilePath))
            {
                using (var stream = File.OpenRead(_XmlFilePath))
                {
                    _Jobs = (List<Job>)serializer.Deserialize(stream);
                }
            }
            else
            {
                _Jobs = new List<Job>();
            }

            timer = new System.Timers.Timer();
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            timer.Interval = 60000;
            timer.AutoReset = false;
            timer.Start();
            timer_Elapsed(null, null);
        }

        public void End()
        {
            lock (this)
            {
                timer.Stop();
            }
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                timer.Stop();
                bool newJob = false;
                try
                {
                    foreach (Job sefiJob in GetSefiJobs())
                    {
                        if (!_Jobs.Exists(job => job.ID == sefiJob.ID && job.Site == "Sefi"))
                        {
                            NewJob(sefiJob);
                            newJob = true;
                            _Jobs.Add(sefiJob);
                        }
                    }
                    foreach (Job tahitiJobsJob in GetTahitiJobJobs())
                    {
                        if (!_Jobs.Exists(job => job.ID == tahitiJobsJob.ID && job.Site == "TahitiJob"))
                        {
                            NewJob(tahitiJobsJob);
                            newJob = true;
                            _Jobs.Add(tahitiJobsJob);
                        }
                    }
                    foreach (Job proInterimJob in GetProInterimJobs())
                    {
                        if (!_Jobs.Exists(job => job.ID == proInterimJob.ID && job.Site == "ProInterim"))
                        {
                            NewJob(proInterimJob);
                            newJob = true;
                            _Jobs.Add(proInterimJob);
                        }
                    }

                    bool trimmed = TrimJobs();
                    if (newJob || trimmed)
                    {
                        using (var stream = File.Create(_XmlFilePath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(List<Job>));
                            serializer.Serialize(stream, _Jobs);
                        }
                    }

                    

                }
                catch (Exception ex)
                {
                    Logger.WriteError("Erreur dans le timer : " + ex.ToString());
                }
                finally { timer.Start(); }
                
            }
        }

        public void NewJob(Job job)
        {
            try
            {
                var status = _TweetManager.SendTweet(string.Format("Offre d'emploi sur le site {0} : {1}. {2}", job.Site, job.Name, job.Url));
                Logger.WriteMessage("Tweet envoyé : " + job.Name);
                //Logger.WriteMessage("Test : job tweeté : " + job.Name);
            }
            catch (Exception e)
            {
                Logger.WriteError("Erreur à l'envoi du tweet : " + e.ToString());
            }
            
        }

        public IEnumerable<Job> GetSefiJobs()
        {
            var request = HttpWebRequest.Create("http://www.sefi.pf/SefiWeb/SefiOffres.nsf/vOffresNouvelles?OpenView");
            var response = request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1")))
            {
                
                string content = streamReader.ReadToEnd();
                int index = 0;
                
                while (true)
                {
                    index = content.IndexOf("/SefiWeb/SefiOffres.nsf/vOffreWeb", index);
                    if (index == -1)
                    {
                        break;
                    }
                    Job j = new Job();
                    j.Site = "Sefi";


                    int debut = index;
                    int fin = content.IndexOf("\"", debut);
                    j.Url = string.Format("http://www.sefi.pf{0}", content.Substring(debut, fin - debut));

                    debut = content.IndexOf("<u>", index);
                    debut += 3;
                    fin = content.IndexOf(" : ", debut);
                    j.ID = content.Substring(debut, fin - debut);

                    debut = fin + 3;
                    fin = content.IndexOf("</u>", debut);
                    j.Name = content.Substring(debut, fin - debut);
                    index = fin;

                    yield return j;
                }
            }
        }



        public IEnumerable<Job> GetTahitiJobJobs()
        {
            var request = HttpWebRequest.Create("http://tahiti.pacifiquejob.com/index.html");
            var response = request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1")))
            {
                string content = streamReader.ReadToEnd();
                int index = 0;
                index = content.IndexOf("Les offres les plus r&eacute;centes", index);

                while (true)
                {
                    index = content.IndexOf("<a id=", index);
                    int max = content.IndexOf("<a href=\"offres.html\"", index);
                    if (index > max)
                    {
                        break;
                    }
                    Job j = new Job();
                    j.Site = "TahitiJob";


                    int debut = content.IndexOf("href=\"", index) + 6;
                    int fin = content.IndexOf("&from", debut);
                    j.Url = string.Format("http://tahiti.pacifiquejob.com/{0}", content.Substring(debut, fin - debut));

                    debut = content.IndexOf("?ID=", debut) + 4;
                    j.ID = content.Substring(debut, fin - debut);

                    debut = content.IndexOf("\">", fin) + 2;
                    fin = content.IndexOf("</a>", debut);
                    if (debut == fin)
                    {
                        index = fin;
                        continue;
                    }
                    j.Name = content.Substring(debut, fin - debut);
                    index = fin;

                    yield return j;
                }
            }

        }


        public IEnumerable<Job> GetProInterimJobs()
        {
            var request = HttpWebRequest.Create("http://www.pro-interim.pf/index.php/offres-d-emplois");
            var response = request.GetResponse();
            using (StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1")))
            {
                string content = streamReader.ReadToEnd();
                int index = 0;
                index = content.IndexOf("Toutes vos offres d'emploi", index);

                while (true)
                {
                   index = content.IndexOf("v-news-item-title", index);

                    if (index == -1)
                    {
                        break;
                    }
                  
                    Job j = new Job();
                    j.Site = "ProInterim";
                    int debut = content.IndexOf("<a href=\"", index) + 9;
                    int fin = content.IndexOf("\">", debut);

                    j.Url = string.Format("http://www.pro-interim.pf{0}", content.Substring(debut, fin - debut));

                    j.ID = content.Substring(debut, fin - debut);

                    debut = content.IndexOf(">", fin) + 1;
                    fin = content.IndexOf("</a>", debut);
                    j.Name = HttpUtility.HtmlDecode(content.Substring(debut, fin - debut));

                    index = fin;
                    yield return j;
                }
            }

        }

        private bool TrimJobs()
        {
            if (_Jobs != null)
            {
                var toRemove = new List<IEnumerable<Job>>();
                foreach (var jobs in (_Jobs.GroupBy(j => j.Site)))
                {
                    if (jobs.Count() > KeepNb)
                    {
                        toRemove.Add(jobs.Take(jobs.Count() - KeepNb));
                    }
                }
                if (toRemove.Count > 0)
                {
                    foreach (var jobs in toRemove)
                    {
                        foreach (var job in jobs)
                        {
                            _Jobs.Remove(job);
                        }
                    }
                    return true;
                }
            }
            return false;
        }



    }
}
