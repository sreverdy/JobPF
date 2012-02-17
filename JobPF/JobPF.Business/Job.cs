using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JobPF.Business
{
    [Serializable]
    public class Job
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Site { get; set; }
    }
}
