using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using nccloudService.DataObjects;
using nccloudService.Models;
using Owin;
using System.Collections.ObjectModel;

namespace nccloudService
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            //For more information on Web API tracing, see http://go.microsoft.com/fwlink/?LinkId=620686 
            config.EnableSystemDiagnosticsTracing();

            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new nccloudInitializer());

            nccloudContext db = new nccloudContext();
            db.Database.Initialize(true);
           
            // To prevent Entity Framework from modifying your database schema, use a null database initializer
            // Database.SetInitializer<nccloudContext>(null);

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                // This middleware is intended to be used locally for debugging. By default, HostName will
                // only have a value when running in an App Service application.
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }
            app.UseWebApi(config);
        }
    }

    public class nccloudInitializer : CreateDatabaseIfNotExists<nccloudContext>
    {
        protected override void Seed(nccloudContext context)
        {
            List<Location> locations = new List<Location>
                {
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Tara Winthrop"},
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Balbriggan Private" },
                    new Location {Id = Guid.NewGuid().ToString(),LocationName="Swords Clinic" }
                };

            foreach (Location location in locations)
            {
                context.Set<Location>().Add(location);
            }
            List<Customer> customers = new List<Customer>
                {
                    new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="ionelpal@gmail.com", CustomerName="Ionel Pal", Location=locations[0] },
                     new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="elenarpal@gmail.com", CustomerName="Elena Pal", Location=locations[2] },
                      new Customer {Id = Guid.NewGuid().ToString(),CustomerEmail="anthonypal37@gmail.com", CustomerName="Ionel Smith", Location=locations[2]}
                };
            foreach (Customer customer in customers)
            {
                context.Set<Customer>().Add(customer);
            }

            List<Patient> patients = new List<Patient>
                {
                    new Patient {Id = Guid.NewGuid().ToString(), PatientName="John Smith", Location=locations[0], Customers =new Collection<Customer> { customers[0]}},
                    new Patient {Id = Guid.NewGuid().ToString(), PatientName="John Smith", Location=locations[1], Customers =new Collection<Customer> { customers[1]}},
                     new Patient {Id = Guid.NewGuid().ToString(), PatientName="John Smith", Location=locations[2], Customers =new Collection<Customer> { customers[2]}}
                };
            foreach (Patient patient in patients)
            {
                context.Set<Patient>().Add(patient);
            }

            List<Message> messages = new List<Message>
                {
                    new Message {Id = Guid.NewGuid().ToString(), Title="Message 1", Details="Message detail 1", WrittebBy="Elena", CustomerEmail="xxxirex@gmail.com",  Patient=patients[0] },
                     new Message {Id = Guid.NewGuid().ToString(), Title="Message 2", Details="Message detail 2", WrittebBy="Adriana",CustomerEmail="ionelpal@gmail.com",  Patient=patients[1] },
                      new Message {Id = Guid.NewGuid().ToString(), Title="Message 3", Details="Message detail 2", WrittebBy="Marie", CustomerEmail="elenarpal@gmail.com", Patient=patients[2] }
                };

            foreach (Message message in messages)
            {
                context.Set<Message>().Add(message);
            }

            base.Seed(context);
        }
    }
}

