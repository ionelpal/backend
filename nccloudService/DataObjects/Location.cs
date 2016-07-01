using Microsoft.Azure.Mobile.Server;
using System.Collections.Generic;

namespace nccloudService.DataObjects
{
    public class Location : EntityData
    {

        //public string LocationId { get; set; } = Guid.NewGuid().ToString();


        public string LocationName { get; set; }
        public string Address { get; set; } 

        public virtual ICollection<Customer> Custommers { get; set; }
      
        public virtual ICollection<Patient> Patients { get; set; }
        public virtual ICollection<Event>Events { get; set; }
    }
}