using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nccloudService.DataObjects
{
  public class Message : EntityData
    {
        public string CustomerEmail { get; set; }
        public string Title { get; set; }   
        public string Details { get; set; }
        public string WrittebBy { get; set; }
        public virtual Patient Patient { get; set; }

    }
}
