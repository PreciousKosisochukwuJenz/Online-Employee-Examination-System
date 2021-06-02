using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineExaminationSystem.DAL.Entity
{
    public class ApplicationSettings
    {
        public int Id { get; set; }
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
