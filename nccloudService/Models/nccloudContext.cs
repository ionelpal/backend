using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using nccloudService.DataObjects;

namespace nccloudService.Models
{
    public class nccloudContext : DbContext
    {
   private const string connectionStringName = "Name=MS_TableConnectionString";

        public nccloudContext() : base(connectionStringName) {}
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Patient> Patients { get; set; }
       
         protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
       new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>
       ("ServiceTableColumn", (property, attributes)
         => attributes.Single().ColumnType.ToString()));
        }
    }

}
