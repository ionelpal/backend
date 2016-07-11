using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nccloudService.DataObjects
{
    public class Event : EntityData
    {
        public string EventTitle { get; set; }
        public string EventDetails { get; set; }
        public DateTime EventDate { get; set; }
        public Location Location { get; set; }
    }
}