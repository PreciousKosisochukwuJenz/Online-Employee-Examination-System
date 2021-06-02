using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineExaminationSystem.Areas.Admin.ViewModels
{
    public class ApplicationSettingsVM
    {
        public int ID { get; set; }
        public string AppName { get; set; }
        public byte[] Logo { get; set; }
        public byte[] Favicon { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FacebookHandle { get; set; }
        public string TwitterHandle { get; set; }
        public string InstagramHandle { get; set; }
        public string LinkedInHandle { get; set; }
   
    }
}