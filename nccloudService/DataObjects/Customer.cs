using Microsoft.Azure.Mobile.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nccloudService.DataObjects
{
    public class Customer : EntityData
    {

        //  public string CustomerId { get; set; } = Guid.NewGuid().ToString();


        public string CustomerEmail { get; set; }

        public string CustomerName { get; set; }
        
        public virtual Location Location { get; set; }
       
        public virtual ICollection<Patient> Patients { get; set; }
    
        public virtual ICollection<Message> Mesages { get; set; }
          
    }
}
